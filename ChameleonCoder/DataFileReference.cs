using System;
using System.IO;
using System.Xml;

namespace ChameleonCoder
{
    internal struct DataFileReference
    {
        internal DataFileReference(Guid id, string path, bool isFile)
            : this()
        {
            Identifier = id;
            Path = path;
            IsFile = isFile;
        }

        internal static DataFileReference CreateReference(XmlElement element)
        {
            string path = element.GetAttribute("path", DataFile.NamespaceUri);
            bool isFile = element.Name == "file";

            if (!string.IsNullOrWhiteSpace(path)
                && (isFile) ? File.Exists(path) : Directory.Exists(path))
            {
                return new DataFileReference(Guid.Parse(element.GetAttribute("id", DataFile.NamespaceUri)),
                    element.InnerText,
                    isFile);
            }
            throw new FileNotFoundException();
        }

        internal Guid Identifier
        {
            get;
            private set;
        }

        public string Path
        {
            get;
            private set;
        }

        internal bool IsFile
        {
            get;
            private set;
        }
    }
}
