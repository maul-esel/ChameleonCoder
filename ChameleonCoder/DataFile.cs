using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// represents an opened resource file
    /// </summary>
    public sealed class DataFile
    {
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

                LoadReferences();
                this.path = path;
            }
            else
                throw new FileNotFoundException("the specified file could not be found", path);
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
                return Document.SelectSingleNode("/cc-resource-file/settings/name").InnerText;
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
            var meta = (XmlElement)Document.SelectSingleNode("/cc-resource-file/settings/metadata[@name='" + key + "']");
            if (meta == null)
            {
                meta = (XmlElement)Document.CreateElement("metadata");
                meta.SetAttribute("name", key);
                Document.SelectSingleNode("/cc-resource-file/settings").AppendChild(meta);
            }

            meta.InnerText = value;
        }

        /// <summary>
        /// gets datafile metadata
        /// </summary>
        /// <param name="key">the metadata's name</param>
        /// <returns>the metadata's value</returns>
        public string GetMetadata(string key)
        {
            var meta = (XmlElement)Document.SelectSingleNode("/cc-resource-file/settings/metadata[@name='" + key + "']");
            if (meta == null)
                return null;

            return meta.InnerText;
        }

        /// <summary>
        /// gets all metadata related to the file
        /// </summary>
        /// <returns>a dictionary containing the metadata</returns>
        public IDictionary<string, string> GetMetadata()
        {
            var set = (XmlElement)Document.SelectSingleNode("/cc-resource-file/settings");
            if (set == null)
                return null;

            var data = set.SelectNodes("metadata");
            var dict = new Dictionary<string, string>();

            foreach (XmlElement meta in data)
            {
                var name = meta.GetAttribute("name");
                if (!string.IsNullOrWhiteSpace(name) && !dict.ContainsKey(name))
                    dict.Add(meta.GetAttribute("name"), meta.InnerText);
            }

            return dict;
        }

        /// <summary>
        /// deletes datafile metadata
        /// </summary>
        /// <param name="key">the metadata's name</param>
        public void DeleteMetadata(string key)
        {
            var meta = (XmlElement)Document.SelectSingleNode("/cc-resource-file/settings/metadata[@name='" + key + "']");
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

            var reference = Document.CreateElement("reference");

            reference.SetAttribute("id", id.ToString("b"));
            reference.SetAttribute("type", isFile ? "file" : "dir");
            reference.InnerText = path;

            Document.SelectSingleNode("/cc-resource-file/references").AppendChild(reference);

            return id;
        }

        /// <summary>
        /// deletes a reference from the DataFile
        /// </summary>
        /// <param name="id">the reference's unique id</param>
        public void DeleteReference(Guid id)
        {
            var reference = Document.SelectSingleNode("/cc-resource-file/references/reference[@id='" + id.ToString("b") + "']");

            if (reference != null)
                reference.ParentNode.RemoveChild(reference);
        }

        /// <summary>
        /// loads the referenced files and directories
        /// </summary>
        private void LoadReferences()
        {
            foreach (XmlElement element in Document.SelectNodes("/cc-resource-file/references/reference"))
            {
                DataFileReference reference;
                try
                {
                    reference = DataFileReference.CreateReference(element);
                }
                catch (FileNotFoundException)
                {
                    continue;
                }

                if (reference.IsFile)
                {
                    Open(reference.Path);
                }
                else
                {
                    Directories.Add(reference.Path);
                }
                References.Add(reference);
            }
        }

        internal IList<DataFileReference> References = new List<DataFileReference>();

        #endregion // references

        /// <summary>
        /// appends the given text to the file's changelog, including an exact date-time stamp
        /// </summary>
        /// <param name="changelog">the text to append</param>
        public void AppendChangelog(string changelog)
        {
            var log = (XmlElement)Document.SelectSingleNode("/cc-resource-file/settings/changelog");

            if (log == null)
            {
                log = Document.CreateElement("changelog");
                Document.SelectSingleNode("/cc-resource-file/settings").AppendChild(log);
            }

            var change = Document.CreateElement("change");
            change.SetAttribute("time", DateTime.Now.ToString("yyyyMMddHHmmss"));
            change.InnerText = changelog;

            log.AppendChild(change);
        }

        /// <summary>
        /// gets a list with the XML representation of all top-level resources in all opened files
        /// </summary>
        /// <returns>the list, containing of XmlElement instances</returns>
        internal IEnumerable<XmlElement> GetResources()
        {
            var elements = new List<XmlElement>();

            foreach (XmlElement element in Document.SelectNodes("/cc-resource-file/resources/*"))
                elements.Add(element);

            return elements;
        }

        #endregion // instance

        #region static

        internal static bool IsLoaded(string path)
        {
            return loadedFilePaths.Contains(path);
        }

        internal static DataFile Open(string path)
        {
            var file = new DataFile(Path.GetFullPath(path));

            loadedFiles.Add(file);
            loadedFilePaths.Add(file.FilePath);
            dirlist.Add(Path.GetDirectoryName(file.FilePath));

            return file;
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
            foreach (DataFile file in LoadedFiles)
                file.Save();
        }

        /// <summary>
        /// closes all loaded instances
        /// </summary>
        internal static void CloseAll()
        {
            foreach (DataFile file in LoadedFiles)
                file.Close();

            LoadedFiles.Clear();
            loadedFilePaths.Clear();
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
            foreach (DataFile file in LoadedFiles)
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
        		return dirlist;
        	}
        }

        internal static IList<DataFile> LoadedFiles
        {
            get
            {
                return loadedFiles;
            }
        }
        
        private static readonly List<string> dirlist = new List<string>(new string[1] { Environment.CurrentDirectory });

        /// <summary>
        /// contains a list of all loaded files in form of their file paths
        /// </summary>
        private static readonly IList<string> loadedFilePaths = new List<string>();

        /// <summary>
        /// contains a list of all loaded files in form of their DataFile instances
        /// </summary>
        private static readonly IList<DataFile> loadedFiles = new List<DataFile>();

        #endregion
    }
}
