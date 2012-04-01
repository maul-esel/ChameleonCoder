using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// represents an opened resource file
    /// </summary>
    public sealed class DataFile
    {
        /// <summary>
        /// the Uri of the resource document schema
        /// </summary>
        public const string NamespaceUri = "ChameleonCoder://Resources/Schema/2011";

        /// <summary>
        /// a template for a new data file
        /// </summary>
        /// <remarks>{0} contains the datafile name, {1} the created data</remarks>
        internal const string fileTemplate = @"<cc:ChameleonCoder xmlns:cc='" + NamespaceUri + "'>"
                                                + "<cc:resources/>"
                                                + "<cc:data/>"
                                                + "<cc:settings>"
                                                    + "<cc:name>{0}</cc:name>"
                                                    + "<cc:created>{1}</cc:created>"
                                                + "</cc:settings>"
                                                + "<cc:references/>"
                                            + "</cc:ChameleonCoder>";

        #region instance

        /// <summary>
        /// creates a new instance of the DataFile class
        /// </summary>
        /// <param name="path">the path to the file</param>
        /// <exception cref="FileFormatException">thrown if the Xml is not valid or could not be read.</exception>
        /// <exception cref="FileNotFoundException">the specified file does not exist</exception>
        private DataFile(string path)
        {
            if (File.Exists(path) && !string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    Document.Load(path);
                }
                catch (XmlException e)
                {
                    throw new FileFormatException(new Uri(path), "Invalid format: not a well-formed XML file.", e);
                }

                manager = NamespaceManagerFactory.GetManager(doc);

                using (var stream = GetType().Assembly.GetManifestResourceStream("ChameleonCoder.schema.xsd"))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        Document.Schemas.Add(null, reader);
                    }
                }

                try
                {
                    Document.Validate(ValidateXmlHandler);
                }
                catch (XmlException)
                {
                    /* the schema does not apply --> inform user + exit */
                    throw;
                }

                LoadReferences();
                this.path = path;
            }
            else
                throw new FileNotFoundException("the specified file could not be found", path);
        }

        private void ValidateXmlHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            if (e.Severity == System.Xml.Schema.XmlSeverityType.Error)
                throw new XmlException(string.Format("The document at {0} is not valid according to the schema!", path), e.Exception);
        }

        /// <summary>
        /// returns the path to the file represented by the instance
        /// </summary>
        public string FilePath { get { return path; } }

        private readonly string path;

        /// <summary>
        /// returns the XmlDocument
        /// </summary>
        internal XmlDocument Document { get { return doc; } }

        private readonly XmlDocument doc = new XmlDocument();

        private readonly XmlNamespaceManager manager;

        /// <summary>
        /// closes the instance
        /// </summary>
        internal void Close() { /* currently we don't need to do something here */ }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        public void Save()
        {
            Document.Save(FilePath);
        }

        /// <summary>
        /// gets the name the user chose for this file
        /// </summary>
        public string Name
        {
            get
            {
                return Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:name", manager).InnerText;
            }
        }

        #region metadata
        /// <summary>
        /// sets datafile metadata, creating it if necessary
        /// </summary>
        /// <param name="key">the metadata's name</param>
        /// <param name="value">the metadata's new value</param>
        public void SetMetadata(string key, string value)
        {
            var meta = (XmlElement)Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta == null)
            {
                meta = (XmlElement)Document.CreateElement("cc:metadata", NamespaceUri);
                meta.SetAttribute("key", NamespaceUri, key);
                Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:metadata", manager).AppendChild(meta);
            }

            meta.SetAttribute("value", NamespaceUri, value);
        }

        /// <summary>
        /// gets datafile metadata
        /// </summary>
        /// <param name="key">the metadata's name</param>
        /// <returns>the metadata's value</returns>
        public string GetMetadata(string key)
        {
            var meta = (XmlElement)Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta == null)
                return null;

            return meta.GetAttribute("value", NamespaceUri);
        }

        /// <summary>
        /// gets all metadata related to the file
        /// </summary>
        /// <returns>a dictionary containing the metadata</returns>
        public IDictionary<string, string> GetMetadata()
        {
            var set = (XmlElement)Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:metadata", manager);
            if (set == null)
                return null;

            var data = set.SelectNodes("cc:metadata", manager);
            var dict = new Dictionary<string, string>();

            foreach (XmlElement meta in data)
            {
                var name = meta.GetAttribute("key", NamespaceUri);
                if (!string.IsNullOrWhiteSpace(name) && !dict.ContainsKey(name))
                    dict.Add(meta.GetAttribute("key", NamespaceUri), meta.GetAttribute("value", NamespaceUri));
            }

            return dict;
        }

        /// <summary>
        /// deletes datafile metadata
        /// </summary>
        /// <param name="key">the metadata's name</param>
        public void DeleteMetadata(string key)
        {
            var meta = (XmlElement)Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta != null)
                meta.ParentNode.RemoveChild(meta);
        }
        #endregion // metadata

        #region references

        /// <summary>
        /// adds a reference to the DataFile
        /// </summary>
        /// <param name="path">the path to the referenced object</param>
        /// <param name="isFile">true if the reference references a file, false it if references a directory</param>
        /// <returns>the reference's uinque id</returns>
        public Guid AddReference(string path, bool isFile)
        {
            var id = Guid.NewGuid();

            var reference = Document.CreateElement(isFile ? "cc:file" : "cc:directory", NamespaceUri);

            reference.SetAttribute("id", NamespaceUri, id.ToString("b"));
            reference.SetAttribute("path", NamespaceUri, path);

            Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:references", manager).AppendChild(reference);

            return id;
        }

        /// <summary>
        /// deletes a reference from the DataFile
        /// </summary>
        /// <param name="id">the reference's unique id</param>
        public void DeleteReference(Guid id)
        {
            var reference = Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:references/cc:reference[@id='" + id.ToString("b") + "']", manager);

            if (reference != null)
                reference.ParentNode.RemoveChild(reference);
        }

        /// <summary>
        /// loads the referenced files and directories
        /// </summary>
        private void LoadReferences()
        {
            foreach (var reference in GetReferences())
            {
                if (reference.IsFile)
                {
                    Open(reference.Path);
                }
                else
                {
                    Directories.Add(reference.Path);
                }
            }
        }

        /// <summary>
        /// gets a list of all referenced files and directories
        /// </summary>
        /// <returns>a list of DataFileReference instances</returns>
        internal IList<DataFileReference> GetReferences()
        {
            var list = new List<DataFileReference>();

            foreach (XmlElement element in Document.SelectNodes("/cc:ChameleonCoder/cc:settings/cc:references/cc:reference", manager))
            {
                try
                {
                    var reference = DataFileReference.CreateReference(element);
                    list.Add(reference);
                }
                catch (FileNotFoundException)
                {
                    continue;
                }
            }

            return list;
        }

        #endregion // references

        /// <summary>
        /// appends the given text to the file's changelog, including an exact date-time stamp
        /// </summary>
        /// <param name="changelog">the text to append</param>
        [Obsolete("not implemented", true)]
        public void AppendChangelog(string changelog)
        {
            throw new NotImplementedException();

            /*var log = (XmlElement)Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:changelog", manager);

            if (log == null)
            {
                log = Document.CreateElement("cc:changelog", NamespaceUri);
                Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings", manager).AppendChild(log);
            }

            var change = Document.CreateElement("cc:change", NamespaceUri);
            change.SetAttribute("time", NamespaceUri, DateTime.Now.ToString("yyyyMMddHHmmss"));
            change.InnerText = changelog;

            log.AppendChild(change);*/
        }

        /// <summary>
        /// gets a list with the XML representation of all top-level resources in all opened files
        /// </summary>
        /// <returns>the list, containing of XmlElement instances</returns>
        private IEnumerable<XmlElement> GetResources()
        {
            var elements = new List<XmlElement>();

            foreach (var node in Document.SelectNodes("/cc:ChameleonCoder/cc:resources/cc:resource", manager))
            {
                var element = node as XmlElement;
                if (element != null)
                {
                    elements.Add(element);
                }
            }

            return elements;
        }

        public bool FileIsLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// parses a XmlElement and its child elements for resource definitions
        /// and creates instances for them, adding them to the global resource list
        /// and to the given parent resource.
        /// </summary>
        /// <param name="node">the XmlElement to parse</param>
        /// <param name="parent">the parent resource or null,
        /// if the resource represented by <paramref name="node"/> is a top-level resource.</param>
        internal void LoadResource(XmlElement node, IResource parent)
        {
            Guid type;
            IResource resource = null;

            if (Guid.TryParse(node.GetAttribute("type", NamespaceUri), out type))
            {
                resource = ResourceTypeManager.CreateInstanceOf(type, node, parent); // try to use the element's name as resource alias
            }
            else if (Guid.TryParse(node.GetAttribute("fallback", NamespaceUri), out type))
            {
                resource = ResourceTypeManager.CreateInstanceOf(type, node, parent); // give it a "2nd chance"
            }

            if (resource == null) // if creation failed:
            {
                ChameleonCoderApp.Log("DataFile  --> internal void LoadResource(XmlElement, IResource)",
                    "failed to create resource",
                    "resource-creation failed on:\n\t" +
                     node.OuterXml + " in " + GetResourceFile(node.OwnerDocument).FilePath); // log
                return; // ignore
            }

            ResourceManager.Add(resource, parent); // and add it to all required lists

            foreach (XmlElement child in node.ChildNodes)
            {
                LoadResource(child, resource); // parse all child resources
            }
            resource.LoadReferences();

            // convert it into a RichContentResource
            IRichContentResource richResource = resource as IRichContentResource;
            if (richResource != null) // if it is really a RichContentResource:
            {
                richResource.MakeRichContent(); // parse the RichContent
            }
        }

        #endregion // instance

        #region static

        internal static bool IsOpen(string path)
        {
            return pathsOpen.Contains(path);
        }

        internal static DataFile Open(string path)
        {
            var file = new DataFile(Path.GetFullPath(path));

            OpenFiles.Add(file);
            pathsOpen.Add(file.FilePath);
            dirList.Add(Path.GetDirectoryName(file.FilePath));

            return file;
        }

        public void Load()
        {
            foreach (XmlElement element in GetResources())
                LoadResource(element, null); // and parse the Xml
            FileIsLoaded = true;
        }

        internal static void LoadAll()
        {
            foreach (var file in OpenFiles)
            {
                if (!file.FileIsLoaded)
                    file.Load();
            }
        }

        /// <summary>
        /// tries to find the absolute path for a given relative path
        /// </summary>
        /// <param name="relativePath">the (relative) path</param>
        /// <returns>the absolute path, or null if not found</returns>
        public static string MakeAbsolutePath(string relativePath)
        {
            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                if (File.Exists(relativePath) || Directory.Exists(relativePath))
                {
                    if (Path.IsPathRooted(relativePath))
                        return relativePath;
                    return Path.GetFullPath(relativePath);
                }

                foreach (var dir in Directories)
                {
                    string fullpath = Path.Combine(dir, relativePath);
                    if (File.Exists(fullpath) || Directory.Exists(fullpath))
                        return fullpath;
                }
            }
            return null;
        }

        /// <summary>
        /// saves all loaded instances
        /// </summary>
        internal static void SaveAll()
        {
            foreach (DataFile file in OpenFiles)
                file.Save();
        }

        /// <summary>
        /// closes all loaded instances
        /// </summary>
        internal static void CloseAll()
        {
            foreach (DataFile file in OpenFiles)
                file.Close();

            OpenFiles.Clear();
            pathsOpen.Clear();
            Directories.Clear();
        }

        /// <summary>
        /// gets the DataFile instance for a given XmlDocument
        /// </summary>
        /// <param name="doc">the XmlDocument</param>
        /// <returns>the DataFile instance</returns>
        /// <exception cref="InvalidOperationException">thrown if the corresponding DataFile is not found</exception>
        internal static DataFile GetResourceFile(XmlDocument doc)
        {
            foreach (DataFile file in OpenFiles)
                if (file.Document == doc)
                    return file;
            throw new InvalidOperationException("this document's resource file cannot be detected:\n\n" + doc.DocumentElement.OuterXml);
        }

        /// <summary>
        /// contains a list of all referenced directories
        /// </summary>
        public static IList<string> Directories
        {
        	get
        	{
        		return dirList;
        	}
        }

        internal static IList<DataFile> OpenFiles
        {
            get
            {
                return filesOpen;
            }
        }
        
        private static readonly List<string> dirList = new List<string>(new string[1] { Environment.CurrentDirectory });

        /// <summary>
        /// contains a list of all loaded files in form of their file paths
        /// </summary>
        private static readonly IList<string> pathsOpen = new List<string>();

        /// <summary>
        /// contains a list of all loaded files in form of their DataFile instances
        /// </summary>
        private static readonly IList<DataFile> filesOpen = new List<DataFile>();

        #endregion
    }
}
