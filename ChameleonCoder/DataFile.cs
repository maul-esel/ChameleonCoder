﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Xml;

namespace ChameleonCoder
{
    public abstract class DataFile
    {
        #region instance

        /// <summary>
        /// serves as base constructor for inherited classes
        /// </summary>
        /// <param name="path">the path to the file</param>
        /// <param name="doc">the XmlDocument containing the file's data</param>
        protected DataFile(string path, XmlDocument doc)
        {
            Document = doc;
            LoadedFilePaths.Add(FilePath = path);
            LoadedFiles.Add(this);
        }

        /// <summary>
        /// returns the path to the file represented by the instance
        /// </summary>
        internal string FilePath { get; private set; }

        /// <summary>
        /// returns the XmlDocument
        /// </summary>
        internal XmlDocument Document { get; private set; }

        /// <summary>
        /// disposes the instance
        /// </summary>
        internal abstract void Dispose();

        /// <summary>
        /// saves the changes made to the file
        /// </summary>
        internal abstract void Save();

        #endregion

        #region static
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
                catch (XmlException e) { throw new FileFormatException(new Uri(path), "Invalid format: not a well-formed XML file.", e); }
            }
            else if (string.Equals(Path.GetExtension(path), ".ccp", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    var pack = Package.Open(path, FileMode.Open, FileAccess.ReadWrite);
                    return new PackDataFile(pack, path);
                }
                catch (FileFormatException e) { throw new FileFormatException(new Uri(path), "Invalid format: not a valid package file.", e); }
            }
            throw new InvalidOperationException("This file could not be opened: " + path + "\nExtension not recognized.");
        }

        /// <summary>
        /// gets a list with the XML representation of all top-level resources in all opened files
        /// </summary>
        /// <returns>the list, containing of XmlElement instances</returns>
        internal static IEnumerable<XmlElement> GetResources()
        {
            var elements = new List<XmlElement>();

            foreach (DataFile file in LoadedFiles)
            {
                foreach (XmlElement element in file.Document.SelectNodes("/cc-resource-file/resources/*"))
                    elements.Add(element);
            }

            return elements;
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
        /// disposes all loaded instances
        /// </summary>
        internal static void DisposeAll()
        {
            foreach (DataFile file in LoadedFiles)
                file.Dispose();

            LoadedFiles.Clear();
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

        internal static readonly IList<string> Directories = new List<string>();

        protected static readonly IList<string> LoadedFilePaths = new List<string>();

        protected static readonly IList<DataFile> LoadedFiles = new List<DataFile>();
        #endregion
    }
}
