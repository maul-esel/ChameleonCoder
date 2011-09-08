using System;
using System.IO;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class FileResource : ResourceBase, IEditable, IFSComponent
    {
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/file.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        #region IEditable

        public string GetText()
        {
            if (!string.IsNullOrWhiteSpace(Path) && File.Exists(Path))
            {
                if (IsBinary(Path))
                    return "file is binary (contains null chars) and can't be loaded";

                return File.ReadAllText(Path);
            }
            return string.Format("path cannot be found: '{0}'", Path);
        }

        public void SaveText(string text)
        {
            if (!string.IsNullOrWhiteSpace(Path))
                if (!IsBinary(this.Path))
                    File.WriteAllText(this.Path, text);
        }

        #endregion

        #region IFSComponent

        public string GetFSPath()
        {
            return Path;
        }

        #endregion

        private bool IsBinary(string path)
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
        /// the path to the file represented by the resource
        /// </summary>
        [ResourceProperty(CommonResourceProperty.FSPath, ResourcePropertyGroup.ThisClass)]
        public string Path
        {
            get
            {
                return Xml.GetAttribute("path");
            }
            protected set
            {
                Xml.SetAttribute("path", value);
                OnPropertyChanged("Path");
            }
        }

        internal const string Alias = "file";
    }
}
