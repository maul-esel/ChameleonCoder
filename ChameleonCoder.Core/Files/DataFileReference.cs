using System;
using System.IO;

namespace ChameleonCoder.Files
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal struct DataFileReference
    {
        internal DataFileReference(Guid id, string path, DataFileReferenceType type)
            : this()
        {
            Identifier = id;
            Path = path;
            Type = type;
        }

        internal static DataFileReference CreateReference(System.Xml.XmlElement element)
        {
            string path = element.GetAttribute("path", DataFile.NamespaceUri);
            DataFileReferenceType type = element.Name == "file" ? DataFileReferenceType.File : DataFileReferenceType.Directory;

            if (!string.IsNullOrWhiteSpace(path)
                && (type == DataFileReferenceType.File)
                    ? File.Exists(path)
                    : Directory.Exists(path))
            {
                return new DataFileReference(Guid.Parse(element.GetAttribute("id", DataFile.NamespaceUri)),
                    element.InnerText,
                    type);
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

        internal DataFileReferenceType Type
        {
            get;
            private set;
        }
    }
}
