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
    [ComVisible(true), ClassInterface(ClassInterfaceType.None), Guid("895F013D-2CD0-4AC0-8AF0-25F727176279")]
    public sealed class DataFile : IDataFile
    {
        #region constants

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

        #endregion // "constants"

        public DataFile()
        {
            manager = XmlNamespaceManagerFactory.GetManager(doc);
        }

        public void Initialize(ChameleonCoderApp app)
        {
            App = app;
        }

        public void Shutdown()
        {
            App = null;
        }

        public void Open(string path)
        {
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

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        public void Save()
        {
            doc.Save(FilePath);
        }

        #region status

        public bool IsInitialized
        {
            get { return App == null; }
        }

        public bool IsOpened
        {
            get { return FilePath == null; }
        }

        /// <summary>
        /// gets whether the file instance is loaded or not
        /// </summary>
        public bool IsLoaded
        {
            get;
            private set;
        }

        #endregion

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
        /// adds a file reference to the instance. Referenced files are opened whenever a file is opened.
        /// </summary>
        /// <param name="referencePath">the path to the file to reference</param>
        /// <returns>the unique id of the reference</returns>
        public Guid AddFileReference(string referencePath)
        {
            Guid id = Guid.NewGuid();
            var reference = doc.CreateElement("cc:file", NamespaceUri);

            reference.SetAttribute("id", NamespaceUri, id.ToString("b"));
            reference.SetAttribute("path", NamespaceUri, referencePath);

            doc.SelectSingleNode(DocumentXPath.ReferenceRoot, manager).AppendChild(reference);
            return id;
        } // todo: instantly load (?)

        /// <summary>
        /// adds a directory reference to the instance. Referenced directories are used to search files.
        /// </summary>
        /// <param name="referencePath">the path to the  directory to reference</param>
        /// <returns>the unique id of the reference</returns>
        public Guid AddDirectoryReference(string referencePath)
        {
            Guid id = Guid.NewGuid();
            var reference = doc.CreateElement("cc:directory", NamespaceUri);

            reference.SetAttribute("id", NamespaceUri, id.ToString("b"));
            reference.SetAttribute("path", NamespaceUri, referencePath);

            doc.SelectSingleNode(DocumentXPath.ReferenceRoot, manager).AppendChild(reference);
            return id;
        } // todo: instantly load (?)

        public string GetFileReference(Guid id)
        {
            var references = doc.SelectSingleNode(DocumentXPath.ReferenceRoot, manager);
            if (references == null)
                throw new InvalidOperationException("No references defined.");

            var element = (XmlElement)references.SelectSingleNode("cc:file[@id='" + id.ToString("b") + "']", manager);
            if (element == null)
                return null;

            return element.GetAttribute("path", NamespaceUri);
        }

        public string GetDirectoryReference(Guid id)
        {
            var references = doc.SelectSingleNode(DocumentXPath.ReferenceRoot, manager);
            if (references == null)
                throw new InvalidOperationException("No references defined.");

            var element = (XmlElement)references.SelectSingleNode("cc:directory[@id='" + id.ToString("b") + "']", manager);
            if (element == null)
                return null;

            return element.GetAttribute("path", NamespaceUri);
        }

        /// <summary>
        /// gets a list of all referenced files and directories
        /// </summary>
        /// <returns>a list of DataFileReference instances</returns>
        public DataFileReference[] GetReferences()
        {
            var list = new List<DataFileReference>();

            foreach (XmlElement element in doc.SelectNodes(DocumentXPath.References, manager))
            {
                list.Add(DataFileReference.CreateReference(element));
            }

            return list.ToArray();
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
                        if (!App.FileMan.IsFileOpen(reference.Path))
                            App.FileMan.OpenFile(reference.Path);
                        break;
                    case DataFileReferenceType.Directory:
                        if (!App.FileMan.IsDirectoryOpen(reference.Path))
                            App.FileMan.OpenDirectory(reference.Path);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion // "references"

        #region resource last modified

        public void ResourceUpdateLastModified(IResource resource)
        {
            ResourceUpdateLastModified(resource, DateTime.Now);
        }

        /// <summary>
        /// updates the "last-modified"-data of a resource to the current time
        /// </summary>
        /// <param name="resource">the resource to update</param>
        public void ResourceUpdateLastModified(IResource resource, DateTime time)
        {
            var res = GetResourceDataElement(resource, true);

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
        public DateTime ResourceGetLastModified(IResource resource)
        {
            var res = GetResourceDataElement(resource, false);
            if (res == null)
                return default(DateTime);

            var lastmod = res.SelectSingleNode("lastmodified");
            if (lastmod == null)
                return default(DateTime);

            return DateTime.Parse(lastmod.InnerText);
        }

        #endregion // "lastmodified"

        #region resource metadata

        /// <summary>
        /// sets the value of a resource's metadata with the specified key and creates it if necessary
        /// </summary>
        /// <param name="resource">the resource to receive the metadata</param>
        /// <param name="key">the metadata key</param>
        /// <param name="value">the value</param>
        public void ResourceSetMetadata(IResource resource, string key, string value)
        {
            // get the resource-data element for the resource
            XmlElement res = GetResourceDataElement(resource, true);

            // get the metadata element for the given key
            XmlElement meta = (XmlElement)res.SelectSingleNode("cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta == null) // if it doesn't exist:
            {
                meta = doc.CreateElement("cc:metadata", NamespaceUri); // create it
                meta.SetAttribute("key", NamespaceUri, key); // give it the requested key
                res.AppendChild(meta); // and insert it
            }

            meta.SetAttribute("value", NamespaceUri, value); // set the value
        }

        /// <summary>
        /// gets the value of a specified metadata element for the resource
        /// </summary>
        /// <param name="resource">the resurce containing the metadata</param>
        /// <param name="key">the metadata's key</param>
        /// <returns>the metadata's value if found, null otherwise</returns>
        public string ResourceGetMetadata(IResource resource, string key)
        {
            // get the resource's data element
            XmlElement res = GetResourceDataElement(resource, false);
            if (res == null) // if it doesn't exist:
                return null; // there's no metadata --> return null

            // get the metadata element
            XmlElement meta = (XmlElement)res.SelectSingleNode("cc:metadata/cc:metadata[@cc:key='" + key + "']", manager);
            if (meta == null) // if it doesn't exist:
                return null; // there's no such metadata --> return null

            return meta.InnerText; // return the requested value
        }

        /// <summary>
        /// gets a list of all metadata elements for the resource
        /// </summary>
        /// <param name="resource">the resource to analyze</param>
        /// <returns>a dictionary containing the metadata, which is empty if None is found</returns>
        public System.Collections.Specialized.StringDictionary ResourceGetMetadata(IResource resource)
        {
            var dict = new System.Collections.Specialized.StringDictionary();

            // get the resource's data element
            XmlElement res = GetResourceDataElement(resource, false);
            if (res == null) // if it doesn't exist:
                return dict; // there's no metadata --> return empty dictionary

            var data = res.SelectNodes("cc:metadata/cc:metadata", manager); // get the list of metadata
            if (data == null)
                return dict;

            foreach (XmlElement meta in data)
            {
                var name = meta.GetAttribute("key", NamespaceUri);
                if (!string.IsNullOrWhiteSpace(name) && !dict.ContainsKey(name))
                {
                    dict.Add(meta.GetAttribute("key", NamespaceUri),
                        meta.GetAttribute("value", NamespaceUri)); // add all metadata elements to the dictionary
                }
            }

            return dict; // return the dictionary
        }

        /// <summary>
        /// deletes a specified metadata
        /// </summary>
        /// <param name="resource">the resource to contain the metadata</param>
        /// <param name="key">the metadata's key</param>
        public void ResourceDeleteMetadata(IResource resource, string key)
        {
            // get the resource's data element
            XmlElement res = GetResourceDataElement(resource, false);
            if (res == null) // if it doesn't exist:
                return; // there's no metadata --> return

            // get the metadata element
            XmlElement meta = (XmlElement)res.SelectSingleNode("cc:metadata/cc:metadata[@cc:key='" + key + "']");
            if (meta == null) // if it doesn't exist:
                return; // there's no such metadata --> return

            meta.ParentNode.RemoveChild(meta); // remove the node
        }

        #endregion // "resource metadata"

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

        public ChameleonCoderApp App
        {
            get;
            private set;
        }

        #region private fields

        [ComVisible(false)]
        private string path = null;

        [ComVisible(false)]
        private readonly XmlDocument doc = new XmlDocument();

        [ComVisible(false)]
        private readonly XmlNamespaceManager manager = null;

        #endregion // private fields

        /// <summary>
        /// returns the XmlDocument
        /// </summary>
        [ComVisible(false), Obsolete]
        internal XmlDocument Document { get { return doc; } }

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
        /// gets the resource-data element for the resource, optionally creating it if not found
        /// </summary>
        /// <param name="resource">the resource whose data should be found</param>
        /// <param name="create">true to create the element if not found, false otherwise</param>
        /// <returns>the XmlElement containing the resource's data</returns>
        [ComVisible(false)]
        private XmlElement GetResourceDataElement(IResource resource, bool create)
        {
            var data = (XmlElement)doc.SelectSingleNode(DocumentXPath.ResourceData + "[@cc:id='" + resource.Identifier.ToString("b") + "']", manager);
            if (data == null && create)
            {
                data = doc.CreateElement("cc:resourcedata", NamespaceUri); // create it
                data.SetAttribute("id", NamespaceUri, resource.Identifier.ToString("b")); // associate it with the resource
                doc.SelectSingleNode(DocumentXPath.DataRoot, manager).AppendChild(data); // and insert it into the document
            }

            return data;
        }

        /// <summary>
        /// parses a XmlElement and its child elements for resource definitions
        /// and creates instances for them, adding them to the global resource list
        /// and to the given parent resource.
        /// </summary>
        /// <param name="node">the XmlElement to parse</param>
        /// <param name="parent">the parent resource or null,
        /// if the resource represented by <paramref name="node"/> is a top-level resource.</param>
        [ComVisible(false), Obsolete("make this private!")]
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

        [ComVisible(false)]
        private void ValidateXmlHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            if (e.Severity == System.Xml.Schema.XmlSeverityType.Error)
                throw new XmlException(string.Format("The document at {0} is not valid according to the schema!", FilePath), e.Exception);
        }
    }
}
