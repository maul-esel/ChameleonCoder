using System;
using System.IO;
using System.IO.Packaging;

namespace ChameleonCoder
{
    internal sealed class PackDataFile : DataFile
    {
        private readonly Package pack;

        private readonly Stream stream;

        /// <summary>
        /// creates a new instance of the PackDataFile class
        /// </summary>
        /// <param name="pack">the Package to open</param>
        /// <param name="path">the path to the file</param>
        internal PackDataFile(Package pack, string path)
            : base(path, new System.Xml.XmlDocument())
        {
            this.pack = pack;
            foreach (var relation in pack.GetRelationshipsByType("ChameleonCoder://Resource/Pack/Markup"))
            {
                var uri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relation.TargetUri);
                var part = pack.GetPart(uri);
                Document.Load(stream = part.GetStream());
                break;
            }
            LoadReferences();
        }

        /// <summary>
        /// disposes the instance
        /// </summary>
        internal override void Dispose()
        {
            stream.Close();
            pack.Close();
        }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        internal override void Save()
        {
            Document.Save(stream);
            // todo: save FSComponents
            pack.Flush();
        }

        /// <summary>
        /// converts a given DataFile instance into a file to be read with the current class
        /// </summary>
        /// <param name="instance">the instance to convert</param>
        /// <returns>the new file's path</returns>
        internal static string Convert(DataFile instance)
        {
            string path = Path.GetTempFileName();

            var newPack = Package.Open(path, FileMode.Create, FileAccess.ReadWrite);

            // todo: add package parts, add fscomponents, ...

            string newpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new resource file");
            int i = 0;
            while (File.Exists(newpath + i + ".ccp"))
                i++;
            newpath += i + ".ccp";

            File.Move(path, newpath);

            return newpath;
        }
    }
}
