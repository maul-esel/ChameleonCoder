using System;
using System.IO;
using System.IO.Packaging;
using System.Xml;

namespace ChameleonCoder
{
    internal abstract class DataFile : IDisposable
    {
        #region instance

        /// <summary>
        /// returns the path to the file represented by the instance
        /// </summary>
        public string FilePath { get; protected internal set; }

        /// <summary>
        /// returns the XmlDocument
        /// </summary>
        public XmlDocument Document { get; protected internal set; }

        /// <summary>
        /// disposes the instance
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        internal abstract void Save();

        #endregion

        /// <summary>
        /// opens a DataFile instance for the given file
        /// </summary>
        /// <param name="path">the path to the file to open</param>
        /// <returns>the DataFile instance</returns>
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
