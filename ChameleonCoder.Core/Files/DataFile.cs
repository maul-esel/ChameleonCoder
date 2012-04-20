using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Files
{
    /// <summary>
    /// represents an opened resource file
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual), Guid("D0EB188C-E42E-4D32-BAB4-9F1F8417EC4D")]
    public sealed class DataFile
    {
        /// <summary>
        /// the Uri of the resource document schema
        /// </summary>
        [DispId(1)]
        public const string NamespaceUri = "ChameleonCoder://Resources/Schema/2011";

        /// <summary>
        /// a template for a new data file
        /// </summary>
        /// <remarks>{0} contains the datafile name, {1} the created data</remarks>
        [ComVisible(false)]
        internal const string fileTemplate = @"<cc:ChameleonCoder xmlns:cc='" + NamespaceUri + "'>"
                                                + "<cc:resources/>"
                                                + "<cc:data/>"
                                                + "<cc:settings>"
                                                    + "<cc:name>{0}</cc:name>"
                                                    + "<cc:created>{1}</cc:created>"
                                                + "</cc:settings>"
                                                + "<cc:references/>"
                                            + "</cc:ChameleonCoder>";

        /// <summary>
        /// creates a new instance of the DataFile class
        /// </summary>
        /// <param name="path">the path to the file</param>
        /// <exception cref="FileFormatException">thrown if the Xml is not valid or could not be read.</exception>
        /// <exception cref="FileNotFoundException">the specified file does not exist</exception>
        public DataFile(string path, ChameleonCoderApp app)
        {
            App = app;
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

                manager = XmlNamespaceManagerFactory.GetManager(doc);

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

        [ComVisible(false)]
        private void ValidateXmlHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            if (e.Severity == System.Xml.Schema.XmlSeverityType.Error)
                throw new XmlException(string.Format("The document at {0} is not valid according to the schema!", FilePath), e.Exception);
        }

        #region private fields

        [ComVisible(false)]
        private readonly string path;

        [ComVisible(false)]
        private readonly XmlDocument doc = new XmlDocument();

        [ComVisible(false)]
        private readonly XmlNamespaceManager manager;

        #endregion // private fields

        #region properties

        /// <summary>
        /// returns the XmlDocument
        /// </summary>
        [ComVisible(false)]
        internal XmlDocument Document { get { return doc; } }

        /// <summary>
        /// returns the path to the file represented by the instance
        /// </summary>
        [DispId(2)]
        public string FilePath { get { return path; } }

        /// <summary>
        /// gets the name the user chose for this file
        /// </summary>
        [DispId(3)]
        public string Name
        {
            get
            {
                return Document.SelectSingleNode(DocumentXPath.SettingName, manager).InnerText;
            }
        }

        /// <summary>
        /// gets whether the file instance is loaded or not
        /// </summary>
        [DispId(4)]
        public bool IsLoaded
        {
            get;
            private set;
        }

        public ChameleonCoderApp App
        {
            get;
            private set;
        }

        #endregion // properties

        #region methods

        /// <summary>
        /// closes the instance
        /// </summary>
        [DispId(5)]
        public void Close()
        {
            App.FileMan.Remove(this);
        }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        [DispId(6)]
        public void Save()
        {
            Document.Save(FilePath);
        }

        /// <summary>
        /// loads the resources contained in the file
        /// </summary>
        [DispId(7)]
        public void Load()
        {
            foreach (XmlNode node in Document.SelectNodes(DocumentXPath.Resources, manager))
            {
                var element = node as XmlElement;
                if (element != null)
                    LoadResource(element, null); // parse the Xml
            }
            IsLoaded = true;
        }

        #region metadata

        /// <summary>
        /// sets datafile metadata, creating it if necessary
        /// </summary>
        /// <param name="key">the metadata's name</param>
        /// <param name="value">the metadata's new value</param>
        [DispId(8)]
        public void SetMetadata(string key, string value)
        {
            var meta = (XmlElement)Document.SelectSingleNode(DocumentXPath.Metadata + "[@cc:key='" + key + "']", manager);
            if (meta == null)
            {
                meta = (XmlElement)Document.CreateElement("cc:metadata", NamespaceUri);
                meta.SetAttribute("key", NamespaceUri, key);
                Document.SelectSingleNode(DocumentXPath.MetadataRoot, manager).AppendChild(meta);
            }

            meta.SetAttribute("value", NamespaceUri, value);
        }

        /// <summary>
        /// gets datafile metadata
        /// </summary>
        /// <param name="key">the metadata's name</param>
        /// <returns>the metadata's value</returns>
        [DispId(9)]
        public string GetMetadata(string key)
        {
            var meta = (XmlElement)Document.SelectSingleNode(DocumentXPath.Metadata + "[@cc:key='" + key + "']", manager);
            if (meta == null)
                return null;

            return meta.GetAttribute("value", NamespaceUri);
        }

        /// <summary>
        /// gets all metadata related to the file
        /// </summary>
        /// <returns>a dictionary containing the metadata</returns>
        [DispId(10)]
        public IDictionary<string, string> GetMetadata()
        {
            var set = (XmlElement)Document.SelectSingleNode(DocumentXPath.MetadataRoot, manager);
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
        [DispId(11)]
        public void DeleteMetadata(string key)
        {
            var meta = (XmlElement)Document.SelectSingleNode(DocumentXPath.Metadata + "[@cc:key='" + key + "']", manager);
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
        [DispId(12)]
        public Guid AddReference(string path, DataFileReferenceType type)
        {
            Guid id = Guid.NewGuid();

            string elementName = null;
            switch (type)
            {
                case DataFileReferenceType.File:
                    elementName = "cc:file"; break;
                case DataFileReferenceType.Directory:
                    elementName = "cc:directory"; break;
                default:
                    throw new NotSupportedException("The given reference type is not known: " + type);
            }
            var reference = Document.CreateElement(elementName, NamespaceUri);

            reference.SetAttribute("id", NamespaceUri, id.ToString("b"));
            reference.SetAttribute("path", NamespaceUri, path);

            Document.SelectSingleNode(DocumentXPath.ReferenceRoot, manager).AppendChild(reference);

            return id;
        }

        /// <summary>
        /// deletes a reference from the DataFile
        /// </summary>
        /// <param name="id">the reference's unique id</param>
        [DispId(13)]
        public void DeleteReference(Guid id)
        {
            var reference = Document.SelectSingleNode(DocumentXPath.References + "[@id='" + id.ToString("b") + "']", manager);

            if (reference != null)
                reference.ParentNode.RemoveChild(reference);
        }

        /// <summary>
        /// loads the referenced files and directories
        /// </summary>
        [ComVisible(false)]
        private void LoadReferences()
        {
            foreach (var reference in GetReferences())
            {
                switch (reference.Type)
                {
                    case DataFileReferenceType.File:
                        App.FileMan.Open(reference.Path);
                        break;
                    case DataFileReferenceType.Directory:
                        App.FileMan.Directories.Add(reference.Path);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// gets a list of all referenced files and directories
        /// </summary>
        /// <returns>a list of DataFileReference instances</returns>
        [DispId(14)]
        internal IList<DataFileReference> GetReferences()
        {
            var list = new List<DataFileReference>();

            foreach (XmlElement element in Document.SelectNodes(DocumentXPath.References, manager))
            {
                var reference = DataFileReference.CreateReference(element);
                list.Add(reference);
            }

            return list;
        }

        #endregion // references

        #endregion // methods

        /// <summary>
        /// appends the given text to the file's changelog, including an exact date-time stamp
        /// </summary>
        /// <param name="changelog">the text to append</param>
        [Obsolete("Not implemented!", true), ComVisible(false)]
        public void AppendChangelog(string changelog)
        {
            throw new NotImplementedException();

            /*
            var log = (XmlElement)Document.SelectSingleNode("/cc:ChameleonCoder/cc:settings/cc:changelog", manager);

            if (log == null)
            {
                log = Document.CreateElement("cc:changelog", NamespaceUri);
                Document.SelectSingleNode(DocumentXPath.Settings, manager).AppendChild(log);
            }

            var change = Document.CreateElement("cc:change", NamespaceUri);
            change.SetAttribute("time", NamespaceUri, DateTime.Now.ToString("yyyyMMddHHmmss"));
            change.InnerText = changelog;

            log.AppendChild(change);
            */
        }

        /// <summary>
        /// parses a XmlElement and its child elements for resource definitions
        /// and creates instances for them, adding them to the global resource list
        /// and to the given parent resource.
        /// </summary>
        /// <param name="node">the XmlElement to parse</param>
        /// <param name="parent">the parent resource or null,
        /// if the resource represented by <paramref name="node"/> is a top-level resource.</param>
        [ComVisible(false)]
        internal void LoadResource(XmlElement node, IResource parent)
        {
            Guid type;
            IResource resource = null;

            if (Guid.TryParse(node.GetAttribute("type", NamespaceUri), out type))
            {
                resource = ResourceTypeManager.CreateInstanceOf(type, node, parent, this); // try to use the element's name as resource alias
            }
            else if (Guid.TryParse(node.GetAttribute("fallback", NamespaceUri), out type))
            {
                resource = ResourceTypeManager.CreateInstanceOf(type, node, parent, this); // give it a "2nd chance"
            }

            if (resource == null) // if creation failed:
            {
                ChameleonCoderApp.Log("DataFile  --> internal void LoadResource(XmlElement, IResource)",
                    "failed to create resource",
                    "resource-creation failed on:\n\t" +
                     node.OuterXml + " in " + parent.File.FilePath); // log
                return; // ignore
            }

            App.ResourceMan.Add(resource, parent); // and add it to all required lists

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
    }
}
