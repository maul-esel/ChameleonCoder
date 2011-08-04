using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class FileResource : ResourceBase, IEditable
    {
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/file.png")).GetAsFrozen() as ImageSource; } }

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

        #region IEditable

        public string GetText()
        {
            if (IsBinary(this.Path))
                return "file is binary (contains null chars) and can't be loaded";

            try { return File.ReadAllText(this.Path); }
            catch (DirectoryNotFoundException) { }
            catch (FileNotFoundException) { }
            return string.Empty;
        }

        public void SaveText(string text)
        {
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
        public string Path
        {
            get
            {
                string path = string.Empty;

                try { path = this.Xml.Attributes["path"].Value; }
                catch (NullReferenceException) { }

                if (!System.IO.Path.IsPathRooted(path) && !string.IsNullOrWhiteSpace(path)
                    && File.Exists(ChameleonCoder.Interaction.InformationProvider.ProgrammingDirectory + path))
                    return ChameleonCoder.Interaction.InformationProvider.ProgrammingDirectory + path;

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
