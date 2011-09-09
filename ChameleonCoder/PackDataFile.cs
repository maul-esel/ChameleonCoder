using System;
using System.IO;
using System.IO.Packaging;
using System.Threading.Tasks;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// represents an opened *.ccp file
    /// </summary>
    internal sealed class PackDataFile : DataFile
    {
        /// <summary>
        /// creates a new instance of the PackDataFile class
        /// </summary>
        /// <param name="pack">the Package to open</param>
        /// <param name="path">the path to the file</param>
        internal PackDataFile(Package pack, string path)
            : base(path, new XmlDocument())
        {
            this.pack = pack;
            foreach (var relation in Package.GetRelationshipsByType("ChameleonCoder://Resource/Pack/Markup"))
            {
                var uri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relation.TargetUri);
                var part = Package.GetPart(uri);
                Document.Load(part.GetStream());
                break;
            }
            LoadReferences();
        }

        /// <summary>
        /// the pack represented by this instance
        /// </summary>
        private readonly Package pack;

        /// <summary>
        /// a thread-safe wrapper property for the pack
        /// </summary>
        private Package Package
        {
            get
            {
                lock (pack)
                {
                    return pack;
                }
            }
        }        

        /// <summary>
        /// closes the instance
        /// </summary>
        internal override void Close()
        {
            Package.Close();
        }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        internal override void Save()
        {
            Document.Save(Package.GetPart(GetPartUri("Markup/Document.xml")).GetStream());
            Package.Flush();
        }

        /// <summary>
        /// adds a file represented by an IFSComponent instance to the DataFile's scope
        /// </summary>
        /// <param name="source">the path to the file</param>
        /// <returns>the Guid that identifies the component inside the DataFile</returns>
        /// <exception cref="FileNotFoundException">the file  pointed to by
        /// <paramref name="source"/> was not found.</exception>
        public override Guid AddFSComponent(string source)
        {
            var id = Guid.NewGuid(); // create a new unique id
            var components = (XmlElement)Document.SelectSingleNode("/cc-resource-file/components"); // get the component registration

            var comp = Document.CreateElement("component"); // add a XmlElement storing the information
            comp.SetAttribute("guid", id.ToString("b")); // ... the id
            comp.SetAttribute("path", source); // ... and the relative / given path

            components.AppendChild(comp); // registrate the file

            string fullpath = MakeAbsolutePath(source); // get the absolute path
            if (fullpath == null)
                throw new FileNotFoundException("The file to add to the DataFile was not found.", source);

            using (var stream = File.OpenRead(fullpath))
            {
                // add the file to the package
                var part = GetPackagePart(pack, "FSComponents/" + id.ToString("b"), stream, "Resource/FSComponent");
                pack.CreateRelationship(part.Uri, TargetMode.Internal, "ChameleonCoder://Resource/Pack/FSComponent");
            }

            return id; // give the id to the caller
        }

        /// <summary>
        /// copies a file represented by an IFSComponent instance inside the DataFile's scope
        /// </summary>
        /// <param name="id">the Guid that identifies the component inside the DataFile</param>
        /// <param name="dest">the destination path</param>
        /// <exception cref="ArgumentException">the component with the given id is not registered</exception>
        /// <exception cref="FileNotFoundException">the source component does not exist</exception>
        public override Guid CopyFSComponent(Guid id, string dest)
        {
            var comp = (XmlElement)Document.SelectSingleNode("/cc-resource-file/components/component[@guid='" + id.ToString("b") + "']");
            if (comp == null)
                throw new ArgumentException("this component is not registered: '" + id.ToString("b") + "'", "id");

            var newId = Guid.NewGuid();

            var copy = Document.CreateElement("component");
            copy.SetAttribute("guid", newId.ToString("b"));
            copy.SetAttribute("path", dest);
            Document.SelectSingleNode("/cc-resource-file/components").AppendChild(copy);

            using (var stream = GetStream(id))
            {
                // add the copy to the package
                var part = GetPackagePart(pack, "FSComponents/" + newId.ToString("b"), stream, "Resource/FSComponent");
                pack.CreateRelationship(part.Uri, TargetMode.Internal, "ChameleonCoder://Resource/Pack/FSComponent");
            }

            return newId;
        }

        /// <summary>
        /// deletes a file represented by an IFSComponent instance from the DataFile's scope
        /// </summary>
        /// <param name="id">the Guid that identifies the component inside the DataFile</param>
        public override void DeleteFSComponent(Guid id)
        {
            var comp = (XmlElement)Document.SelectSingleNode("/cc-resource-file/components/component[@guid='" + id.ToString("b") + "']");
            if (comp != null)
            {
                File.Delete(MakeAbsolutePath(comp.GetAttribute("path")));

                comp.ParentNode.RemoveChild(comp);
            }
            pack.DeletePart(GetPartUri("FSComponents/" + id.ToString("b")));
        }

        /// <summary>
        /// determines whether the given component exists,
        /// that means it is registered and the content exists, too.
        /// </summary>
        /// <param name="id">the Guid that identifies the component inside the DataFile</param>
        /// <returns>true is the component exists, false otherwise</returns>
        public override bool Exists(Guid id)
        {
            var comp = (XmlElement)Document.SelectSingleNode("/cc-resource-file/components/component[@guid='" + id.ToString("b") + "']");
            return (comp != null && pack.PartExists(GetPartUri("FSComponents/" + id.ToString("b"))));
        }

        /// <summary>
        /// gets a stream containing the file's content
        /// </summary>
        /// <param name="id">the Guid that identifies the component inside the DataFile</param>
        /// <returns>the content as stream</returns>
        /// <exception cref="FileNotFoundException">the source component does not exist</exception>
        public override Stream GetStream(Guid id)
        {
            var uri = GetPartUri("FSComponents/" + id.ToString("b"));
            if (!pack.PartExists(uri))
                throw new FileNotFoundException();
            return pack.GetPart(uri).GetStream();
        }

        /// <summary>
        /// converts a given DataFile instance into a file to be read with the current class
        /// </summary>
        /// <param name="instance">the instance to convert</param>
        /// <returns>the new file's path</returns>
        internal static string Convert(DataFile instance)
        {
            string path = Path.GetTempFileName();

            using (var newPack = Package.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(instance.Document.DocumentElement.OuterXml);
                        PackagePart docPart = GetPackagePart(newPack, "Markup/Document.xml", stream, "Resource/Markup");
                        newPack.CreateRelationship(docPart.Uri, TargetMode.Internal, "ChameleonCoder://Resource/Pack/Markup");
                    }
                }
                AddFSComponents(res => res.GetResourceFile() == instance, newPack);
                // todo: use <components> section instead
            }

            // todo: move & ASSIGN *.CCP EXTENSION!
            
            return path;
        }

        #region pack helper methods

        /// <summary>
        /// gets a package part for the given Uri,
        /// creating it if not found.
        /// </summary>
        /// <param name="zip">the package containing the part</param>
        /// <param name="uri">the Uri-String to the part</param>
        /// <param name="content">the content to set for the part (if created)</param>
        /// <param name="contentType">the contentType to set for the part (if created)</param>
        /// <returns>the package part</returns>
        private static PackagePart GetPackagePart(Package zip, string uri, Stream content, string contentType)
        {
            Uri target = GetPartUri(uri);

            if (zip.PartExists(target))
                return zip.GetPart(target);

            PackagePart part = zip.CreatePart(target, contentType, CompressionOption.Maximum);
            content.CopyTo(part.GetStream());

            return part;
        }

        /// <summary>
        /// gets the part-Uri for a given string
        /// </summary>
        /// <param name="path">the path as string</param>
        /// <returns>the path as part-Uri</returns>
        private static Uri GetPartUri(string path)
        {
            return PackUriHelper.CreatePartUri(new Uri(Uri.EscapeDataString(path), UriKind.Relative));
        }

        /// <summary>
        /// adds the FSComponents to a package
        /// </summary>
        /// <param name="filter">a delegate to filter the resources</param>
        /// <param name="zip">the package receiving the fscomponents</param>
        private static void AddFSComponents(Predicate<Resources.Interfaces.IFSComponent> filter, Package zip)
        {
            Parallel.ForEach(Resources.Management.ResourceManager.GetList(),
                resource =>
            {
                var fscomponent = resource as Resources.Interfaces.IFSComponent;
                if (fscomponent != null && filter(fscomponent))
                {
                    string fullpath = MakeAbsolutePath(fscomponent.GetFSPath());
                    if (File.Exists(fullpath))
                    {
                        using (Stream stream = File.OpenRead(fullpath))
                        {

                            if (stream != null)
                            {
                                var part = GetPackagePart(zip, "FSComponents/" + fscomponent.GetFSPath(), stream, "Resource/FSComponent");
                                zip.CreateRelationship(part.Uri, TargetMode.Internal, "ChameleonCoder://Resource/Pack/FSComponent");
                            }
                        }
                    }
                }
            });
        }

        #endregion

    }
}
