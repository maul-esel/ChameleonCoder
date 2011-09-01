using System.Xml;

namespace ChameleonCoder
{
    internal sealed class XmlDataFile : DataFile
    {
        internal XmlDataFile(XmlDocument doc, string path)
            : base(path, doc)
        {
        }

        public override void Dispose()
        {
        }

        internal override void Save()
        {
            Document.Save(FilePath);
        }
    }
}
