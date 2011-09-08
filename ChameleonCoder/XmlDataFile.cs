using System;
using System.IO;
using System.Xml;

namespace ChameleonCoder
{
    internal sealed class XmlDataFile : DataFile
    {
        /// <summary>
        /// creates a new instance of the XmlDataFile instance
        /// </summary>
        /// <param name="doc">the document</param>
        /// <param name="path">the file</param>
        internal XmlDataFile(XmlDocument doc, string path)
            : base(path, doc)
        {
            foreach (XmlElement reference in Document.SelectNodes("/cc-resource-file/references/reference"))
            {
                if (reference.GetAttribute("type") == "dir" && !string.IsNullOrWhiteSpace(reference.InnerText)
                    && Directory.Exists(reference.InnerText))
                    Directories.Add(reference.InnerText);
                else if (reference.GetAttribute("type") == "file" && !string.IsNullOrWhiteSpace(reference.InnerText)
                    && File.Exists(reference.InnerText))
                    DataFile.Open(reference.InnerText);
            }
        }

        /// <summary>
        /// disposes the instance
        /// </summary>
        internal override void Dispose()
        {
        }

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        internal override void Save()
        {
            Document.Save(FilePath);
        }

        /// <summary>
        /// converts a given DataFile instance into a file to be read with the current class
        /// </summary>
        /// <param name="instance">the instance to convert</param>
        /// <returns>the new file's path</returns>
        internal static string Convert(DataFile instance)
        {
            string path = Path.GetTempFileName();

            instance.Document.Save(path);

            string newpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "new resource file");
            int i = 0;
            while (File.Exists(newpath + i + ".ccr"))
                i++;
            newpath += i + ".ccr";

            File.Move(path, newpath);

            return newpath;
        }
    }
}
