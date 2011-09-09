using System;
using System.IO;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// represents an opened *.ccr file
    /// </summary>
    internal sealed class XmlDataFile : DataFile
    {
        /// <summary>
        /// creates a new instance of the XmlDataFile instance
        /// </summary>
        /// <param name="doc">the document</param>
        /// <param name="path">the file</param>
        internal XmlDataFile(XmlDocument doc, string path)
            : base(path, doc)
        {
            LoadReferences();
        }

        /// <summary>
        /// closes the instance
        /// </summary>
        internal override void Close()
        {
        }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        internal override void Save()
        {
            Document.Save(FilePath);
        }

        /// <summary>
        /// adds a file represented by an IFSComponent instance to the DataFile's scope
        /// </summary>
        /// <param name="source">the path to the file</param>
        /// <returns>the Guid that identifies the component inside the DataFile</returns>
        public override Guid AddFSComponent(string source)
        {
            var id = Guid.NewGuid(); // create a new unique id
            var components = (XmlElement)Document.SelectSingleNode("/cc-resource-file/components"); // get the component registration

            var comp = Document.CreateElement("component"); // add a XmlElement storing the information
            comp.SetAttribute("guid", id.ToString("b")); // ... the id
            comp.SetAttribute("path", source); // ... and the relative / given path

            components.AppendChild(comp); // registrate the file

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

            string source = Path.GetFullPath(comp.GetAttribute("path"));
            if (File.Exists(source))
            {
                File.Copy(source, dest, true);

                var newId = Guid.NewGuid();

                var copy = Document.CreateElement("component");
                copy.SetAttribute("guid", newId.ToString("b"));
                copy.SetAttribute("path", dest);
                Document.SelectSingleNode("/cc-resource-file/components").AppendChild(copy);

                return newId;
            }
            throw new FileNotFoundException("the file with the id '" + id.ToString("b") + "' does not exist.", source);
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
            if (comp == null)
                return false;

            return File.Exists(MakeAbsolutePath(comp.GetAttribute("path")));
        }

        /// <summary>
        /// gets a stream containing the file's content
        /// </summary>
        /// <param name="id">the Guid that identifies the component inside the DataFile</param>
        /// <returns>the content as stream</returns>
        /// <exception cref="ArgumentException">the component with the given id is not registered</exception>
        /// <exception cref="FileNotFoundException">the source component does not exist</exception>
        public override Stream GetStream(Guid id)
        {
            var comp = (XmlElement)Document.SelectSingleNode("/cc-resource-file/components/component[@guid='" + id.ToString("b") + "']");
            if (comp == null)
                throw new ArgumentException("this component is not registered: '" + id.ToString("b") + "'", "id");

            string path = MakeAbsolutePath(comp.GetAttribute("path"));
            if (path == null)
                throw new FileNotFoundException("the file with the id '" + id.ToString("b") + "' does not exist.", comp.GetAttribute("path"));

            return File.Open(path, FileMode.Open, FileAccess.ReadWrite);
        }

        /// <summary>
        /// converts a given DataFile instance into a file to be read with the current class
        /// </summary>
        /// <param name="instance">the instance to convert</param>
        /// <returns>the new file's path</returns>
        internal static string Convert(DataFile instance)
        {
            string path = Path.GetTempFileName();

            instance.Document.Save(path);

            string newpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new resource file");
            int i = 0;
            while (File.Exists(newpath + i + ".ccr"))
                i++;
            newpath += i + ".ccr";

            File.Move(path, newpath);

            return newpath;
        }
    }
}
