using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Xml;

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

        #region IEnumerable

        public override IEnumerator<PropertyDescription> GetEnumerator()
        {
            IEnumerator<PropertyDescription> baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            yield return new PropertyDescription("path", this.Path, "File");
        }

        #endregion

        /// <summary>
        /// the path to the file represented by the resource
        /// </summary>
        public string Path
        {
            get
            {
                string path = string.Empty;

                try { path = this.Xml.Attributes["path"].Value; }
                catch (NullReferenceException) { }

                if (!System.IO.Path.IsPathRooted(path) && !string.IsNullOrWhiteSpace(path)
                    && System.IO.File.Exists(Properties.Settings.Default.ScriptDir + path))

                    return ChameleonCoder.Properties.Settings.Default.ScriptDir + path;
                return path;
            }
            protected set
            {
                this.Xml.Attributes["path"].Value = value;
                this.OnPropertyChanged("Path");
            }
        }
    }
}
