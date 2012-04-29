using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Files;
using ChameleonCoder.Resources;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class FileResource : ResourceBase, IEditable, IFSComponent
    {
        #region IResource

        /// <summary>
        /// gets the icon representing this resource to the user
        /// </summary>
        /// <value>This is always the same as the FileResource's type icon.</value>
        public override ImageSource Icon
        {
            get
            {
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/file.png"))
                    .GetAsFrozen() as ImageSource;
            }
        }

        #endregion

        #region IEditable

        /// <summary>
        /// gets the text represented by this resource, to be edited by the user
        /// </summary>
        /// <returns>the content of the file represented by this resource,
        /// or an error message if the text could not be btained (missing or binary file).</returns>
        public string GetText()
        {
            if (!string.IsNullOrWhiteSpace(Path) && System.IO.File.Exists(Path))
            {
                if (IsBinary(Path))
                    return "file is binary (contains null chars) and can't be loaded";

                return System.IO.File.ReadAllText(Path);
            }
            return string.Format("path cannot be found: '{0}'", Path);
        }

        /// <summary>
        /// saves the text represented by this resource, edited by the user
        /// </summary>
        /// <param name="text">the modified contents of the file</param>
        public void SaveText(string text)
        {
            if (!string.IsNullOrWhiteSpace(Path))
                if (!IsBinary(Path))
                    System.IO.File.WriteAllText(Path, text);
        }

        #endregion

        #region IFSComponent

        /// <summary>
        /// gets the resource's path in the file system
        /// </summary>
        /// <returns>the path as string</returns>
        public string GetFSPath()
        {
            return Path;
        }

        #endregion

        private static bool IsBinary(string path)
        {
            using (FileStream s = new FileStream(path, FileMode.OpenOrCreate))
            {
                for (int i = 0; i < 100; i++)
                    if (s.ReadByte() == byte.MinValue)
                        return true;
            }
            return false;
        }

        /// <summary>
        /// gets the path to the file represented by the resource
        /// </summary>
        /// <value>The value is taken from the "path" attirubte in the resource's XML.</value>
        [ResourceProperty(CommonResourceProperty.FSPath, ResourcePropertyGroup.ThisClass)]
        public string Path
        {
            get
            {
                return Attributes["path"]; // Xml.GetAttribute("path", DataFile.NamespaceUri);
            }
            protected set
            {
                Attributes["path"] = value; // Xml.SetAttribute("path", DataFile.NamespaceUri, value);
                OnPropertyChanged("Path");
            }
        }

        internal const string Key = "{826ab0e7-00d5-45be-88e5-7bb2f1d565ab}";
    }
}
