using System;
using System.Windows.Media;
using System.Collections.Generic;
using System.Xml;
using CC = ChameleonCoder.Resources;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class FileResource : ResourceBase
    {
        public FileResource(XmlNode node, CC.Interfaces.IResource parent)
            : base(node, parent)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/file.png")); } }

        #endregion

        #region IEnumerable

        public override IEnumerator<CC.PropertyDescription> GetEnumerator()
        {
            IEnumerator<CC.PropertyDescription> baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            yield return new CC.PropertyDescription("path", this.Path, "File");
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
                    && System.IO.File.Exists(ChameleonCoder.InformationProvider.ProgrammingDirectory + path))
                    return ChameleonCoder.InformationProvider.ProgrammingDirectory + path;

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
