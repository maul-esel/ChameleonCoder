using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class FileResource : ResourceBase
    {
        public FileResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/file.png")); } }

        #endregion

        /// <summary>
        /// the path to the file represented by the resource
        /// </summary>
        public string Path
        {
            get
            {
                string path = string.Empty;

                try { path = this.XMLNode.Attributes["path"].Value; }
                catch (NullReferenceException) { }

                if (!System.IO.Path.IsPathRooted(path) && path != string.Empty
                    && System.IO.File.Exists(Properties.Settings.Default.ScriptDir + path))

                    return ChameleonCoder.Properties.Settings.Default.ScriptDir + path;
                return path;
            }
            protected set
            {
                this.XMLNode.Attributes["path"].Value = value;
                this.OnPropertyChanged("Path");
            }
        }
    }
}
