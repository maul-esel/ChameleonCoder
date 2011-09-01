using System;
using System.IO.Packaging;
using System.Xml;

namespace ChameleonCoder
{
    internal sealed class PackDataFile : DataFile
    {
        private readonly Package pack;

        private readonly System.IO.Stream stream;

        internal PackDataFile(Package pack, string path)
            : base(path)
        {
            this.pack = pack;
            foreach (var relation in pack.GetRelationshipsByType("ChameleonCoder://Resource/Pack/Markup"))
            {
                var uri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relation.TargetUri);
                var part = pack.GetPart(uri);
                Document.Load(stream = part.GetStream());
                break;
            }            
        }

        public override void Dispose()
        {
            stream.Close();
            pack.Close();
        }

        internal override void Save()
        {
            Document.Save(stream);
            pack.Flush();
        }
    }
}
