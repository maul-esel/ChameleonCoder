using System;
using System.IO;
using System.Xml;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Interaction
{
    public static class InformationProvider
    {
        public static string ProgrammingDirectory { get { return Properties.Settings.Default.ProgrammingDir; } }
        public static int Language { get { return Properties.Settings.Default.Language; } }
        public static string AppDir { get { return App.AppDir; } }
        public static string DataDir { get { return App.DataDir; } }

        public static IResource GetInstance(Guid id)
        {
            return ResourceManager.GetList().GetInstance(id);
        }

        public static ResourceTypeInfo GetInfo(Type type)
        {
            return ResourceTypeManager.GetInfo(type);
        }

        public static string FindFreePath(string directory, string baseName, bool isFile)
        {
            directory = directory[directory.Length - 1] == Path.DirectorySeparatorChar
                ? directory : directory + Path.DirectorySeparatorChar;

            baseName = baseName.TrimStart(Path.DirectorySeparatorChar);

            string fileName = isFile
                ? Path.GetFileNameWithoutExtension(baseName) : baseName;

            string Extension = isFile
                ? Path.GetExtension(baseName) : string.Empty;

            string path = directory + fileName + Extension;
            int i = 0;

            while ((isFile ? File.Exists(path) : Directory.Exists(path)))
            {
                path = directory + fileName + "_" + i + Extension;
                i++;
            }

            return path;
        }

        public static XmlElement CloneElement(XmlElement element, XmlDocument newOwner)
        {
            XmlElement newElement = newOwner.CreateElement(element.Name);
            foreach (XmlAttribute attr in element.Attributes)
            {
                XmlAttribute newAttr = newOwner.CreateAttribute(attr.Name);
                newAttr.Value = attr.Value;
                newElement.SetAttributeNode(newAttr);
            }
            foreach (XmlElement child in element.ChildNodes)
            {
                XmlElement newChild = CloneElement(child, newOwner);
                newElement.AppendChild(newChild);
            }
            return newElement;
        }
    }
}
