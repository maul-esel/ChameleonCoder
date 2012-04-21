using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Files
{
    /// <summary>
    /// represents an opened resource file
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
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
                    doc.Load(path);
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
                        doc.Schemas.Add(null, reader);
                    }
                }

                try
                {
                    doc.Validate(ValidateXmlHandler);
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
        [ComVisible(false), Obsolete]
        internal XmlDocument Document { get { return doc; } }

        /// <summary>
        /// returns the path to the file represented by the instance
        /// </summary>
        public string FilePath { get { return path; } }

        /// <summary>
        /// gets the name the user chose for this file
        /// </summary>
        public string Name
        {
            get
            {
                return doc.SelectSingleNode(DocumentXPath.SettingName, manager).InnerText;
            }
        }

        /// <summary>
        /// gets whether the file instance is loaded or not
        /// </summary>
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
        public void Close()
        {
            App.FileMan.Remove(this);
        }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        public void Save()
        {
            doc.Save(FilePath);
        }

        /// <summary>
        /// loads the resources contained in the file
        /// </summary>
        public void Load()
        {
            foreach (XmlNode node in doc.SelectNodes(DocumentXPath.Resources, manager))
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
        public void SetMetadata(string key, string value)
        {
            var meta = (XmlElement)doc.SelectSingleNode(DocumentXPath.Metadata + "[@cc:key='" + key + "']", manager);
            if (meta == null)
            {
                meta = (XmlElement)doc.CreateElement("cc:metadata", NamespaceUri);
                meta.SetAttribute("key", NamespaceUri, key);
                doc.SelectSingleNode(DocumentXPath.MetadataRoot, manager).AppendChild(meta);
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
            var meta = (XmlElement)doc.SelectSingleNode(DocumentXPath.Metadata + "[@cc:key='" + key + "']", manager);
            if (meta == null)
                return null;

            return meta.GetAttribute("value", NamespaceUri);
        }

        /// <summary>
        /// gets all metadata related to the file
        /// </summary>
        /// <returns>a dictionary containing the metadata</returns>
        public System.Collections.Specialized.StringDictionary GetMetadata()
        {
            var set = (XmlElement)doc.SelectSingleNode(DocumentXPath.MetadataRoot, manager);
            if (set == null)
                return null;

            var data = set.SelectNodes("cc:metadata", manager);
            var dict = new System.Collections.Specialized.StringDictionary();

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
            var meta = (XmlElement)doc.SelectSingleNode(DocumentXPath.Metadata + "[@cc:key='" + key + "']", manager);
            if (meta != null)
                meta.ParentNode.RemoveChild(meta);
        }

        #endregion // "metadata"

        #region references

        /// <summary>
        /// adds a reference to the DataFile
        /// </summary>
        /// <param name="path">the path to the referenced object</param>
        /// <param name="isFile">true if the reference references a file, false it if references a directory</param>
        /// <returns>the reference's uinque id</returns>
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
            var reference = doc.CreateElement(elementName, NamespaceUri);

            reference.SetAttribute("id", NamespaceUri, id.ToString("b"));
            reference.SetAttribute("path", NamespaceUri, path);

            doc.SelectSingleNode(DocumentXPath.ReferenceRoot, manager).AppendChild(reference);

            return id;
        }

        /// <summary>
        /// deletes a reference from the DataFile
        /// </summary>
        /// <param name="id">the reference's unique id</param>
        public void DeleteReference(Guid id)
        {
            var reference = doc.SelectSingleNode(DocumentXPath.References + "[@id='" + id.ToString("b") + "']", manager);

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
                        App.FileMan.OpenFile(reference.Path);
                        break;
                    case DataFileReferenceType.Directory:
                        App.FileMan.OpenDirectory(reference.Path);
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
        internal IList<DataFileReference> GetReferences()
        {
            var list = new List<DataFileReference>();

            foreach (XmlElement element in doc.SelectNodes(DocumentXPath.References, manager))
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

        #region "old ResourceHelper"

        #region lastmodified

        public void UpdateResourceLastModified(IResource resource)
        {
            UpdateResourceLastModified(resource, DateTime.Now);
        }

        /// <summary>
        /// updates the "last-modified"-data of a resource to the current time
        /// </summary>
        /// <param name="resource">the resource to update</param>
        public void UpdateResourceLastModified(IResource resource, DateTime time)
        {
            var res = GetResourceDataElement(resource, true);
            var manager = XmlNamespaceManagerFactory.GetManager(res.OwnerDocument);

            var lastmod = res.SelectSingleNode("cc:lastmodified", manager);

            if (lastmod == null)
            {
                lastmod = res.OwnerDocument.CreateElement("cc:lastmodified", NamespaceUri);
                res.AppendChild(lastmod);
            }

            lastmod.InnerText = time.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// gets the "last-modified"-data of a resource
        /// </summary>
        /// <param name="resource">the resource to analyze</param>
        /// <returns>the last-modified DateTime, or <code>default(DateTime)</code> if it couldn't be found.</returns>
        public DateTime GetResourceLastModified(IResource resource)
        {
            var res = GetResourceDataElement(resource, false);
            if (res == null)
                return default(DateTime);

            var lastmod = res.SelectSingleNode("lastmodified");
            if (lastmod == null)
                return default(DateTime);

            return DateTime.Parse(lastmod.InnerText);
        }

        #endregion // "old ResourceHelper" > "lastmodified"

        #endregion // "old ResourceHelper"

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
                resource = App.ResourceTypeMan.CreateInstanceOf(type, node, parent, this); // try to use the element's name as resource alias
            }
            else if (Guid.TryParse(node.GetAttribute("fallback", NamespaceUri), out type))
            {
                resource = App.ResourceTypeMan.CreateInstanceOf(type, node, parent, this); // give it a "2nd chance"
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
