using System;
using System.Xml;

namespace ChameleonCoder
{
    public static class DataFileHelper
    {
        #region metadata
        /// <summary>
        /// sets datafile metadata, creating it if necessary
        /// </summary>
        /// <param name="file">the DataFile to work on</param>
        /// <param name="key">the metadata's name</param>
        /// <param name="value">the metadata's new value</param>
        public static void SetMetadata(this DataFile file, string key, string value)
        {
            var meta = (XmlElement)file.Document.SelectSingleNode("/cc-resource-file/settings/metadata[@name='" + key + "']");
            if (meta == null)
            {
                meta = (XmlElement)file.Document.CreateElement("metadata");
                meta.SetAttribute("name", key);
                file.Document.SelectSingleNode("/cc-resource-file/settings").AppendChild(meta);
            }

            meta.InnerText = value;
        }

        /// <summary>
        /// gets datafile metadata
        /// </summary>
        /// <param name="file">the DataFile to work on</param>
        /// <param name="key">the metadata's name</param>
        /// <returns>the metadata's value</returns>
        public static string GetMetadata(this DataFile file, string key)
        {
            var meta = (XmlElement)file.Document.SelectSingleNode("/cc-resource-file/settings/metadata[@name='" + key + "']");
            if (meta == null)
                return null;

            return meta.InnerText;
        }

        /// <summary>
        /// deletes datafile metadata
        /// </summary>
        /// <param name="file">the DataFile to work on</param>
        /// <param name="key">the metadata's name</param>
        public static void DeleteMetadata(this DataFile file, string key)
        {
            var meta = (XmlElement)file.Document.SelectSingleNode("/cc-resource-file/settings/metadata[@name='" + key + "']");
            if (meta != null)
                meta.ParentNode.RemoveChild(meta);
        }
        #endregion

        #region references
        /// <summary>
        /// adds a reference to the DataFile
        /// </summary>
        /// <param name="file">the DataFile to work on</param>
        /// <param name="path">the path to the referenced object</param>
        /// <param name="isFile">true if the reference references a file, false it if references a directory</param>
        /// <returns>the reference's uinque id</returns>
        public static Guid AddReference(this DataFile file, string path, bool isFile)
        {
            var id = Guid.NewGuid();

            var reference = file.Document.CreateElement("reference");

            reference.SetAttribute("id", id.ToString("n"));
            reference.SetAttribute("type", isFile ? "file" : "dir");
            reference.InnerText = path;

            file.Document.SelectSingleNode("/cc-resource-file/references").AppendChild(reference);

            return id;
        }

        /// <summary>
        /// deletes a reference from the DataFile
        /// </summary>
        /// <param name="file">the DataFile to work on</param>
        /// <param name="id">the reference's unique id</param>
        public static void DeleteReference(this DataFile file, Guid id)
        {
            var reference = file.Document.SelectSingleNode("/cc-resource-file/references/reference[@id='" + id.ToString("n") + "']");

            if (reference != null)
                reference.ParentNode.RemoveChild(reference);
        }
        #endregion
    }
}
