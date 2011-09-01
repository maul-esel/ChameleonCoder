using System;
using System.IO;
using System.IO.Packaging;
using System.Xml;

namespace ChameleonCoder
{
    internal abstract class DataFile : IDisposable
    {
        #region instance
        internal readonly string FilePath;

        internal XmlDocument Document { get; private set; }

        internal DataFile(string path)
            : this(path, new XmlDocument())
        {            
        }

        internal DataFile(string path, XmlDocument doc)
        {
            FilePath = path;
            Document = doc;
        }

        public abstract void Dispose();

        internal abstract void Save();
        #endregion

        internal static DataFile Open(string path)
        {
            if (string.Equals(Path.GetExtension(path), ".ccr", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(path);
                    return new XmlDataFile(doc, path);
                }
                catch (XmlException) { throw; }
            }
            else if (string.Equals(Path.GetExtension(path), ".ccp", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var pack = Package.Open(path, FileMode.Open, FileAccess.ReadWrite);
                    return new PackDataFile(pack, path);
                }
                catch (FileFormatException) { throw; }
            }
            throw new InvalidOperationException("This file could not be opened: " + path + "\nExtension not recognized.");
        }
    }
}
