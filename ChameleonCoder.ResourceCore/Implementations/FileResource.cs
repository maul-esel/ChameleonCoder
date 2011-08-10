using System;
using System.IO;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class FileResource : ResourceBase, IEditable // todo: implement IFSComponent
    {
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/file.png")).GetAsFrozen() as ImageSource; } }

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
                string path = Xml.GetAttribute("path");

                if (!System.IO.Path.IsPathRooted(path) && !string.IsNullOrWhiteSpace(path)
                    && File.Exists(ChameleonCoder.Interaction.InformationProvider.ProgrammingDirectory + path))
                    return ChameleonCoder.Interaction.InformationProvider.ProgrammingDirectory + path;

                return path;
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
