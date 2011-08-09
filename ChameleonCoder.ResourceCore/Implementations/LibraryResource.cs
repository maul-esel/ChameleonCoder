using System;
using System.Windows.Media;
using System.Xml;
using CC = ChameleonCoder.Resources;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// represents a library resource,
    /// inherits from CodeResource
    /// </summary>
    public class LibraryResource : CodeResource
    {
        /// <summary>
        /// creates a new instance of the LibraryResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public override void Init(XmlElement node, CC.Interfaces.IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/library.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        [Resources.ResourceProperty("nameof_Author", CC.ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Author
        {
            get
            {
                try { return this.Xml.Attributes["author"].Value; }
                catch (NullReferenceException) { return null; }
            }
            set
            {
                this.Xml.Attributes["author"].Value = value;
                this.OnPropertyChanged("Author");
            }
        }

        [Resources.ResourceProperty("nameof_License", CC.ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string License
        {
            get
            {
                try { return this.Xml.Attributes["license"].Value; }
                catch (NullReferenceException) { return null; }
            }
            set
            {
                this.Xml.Attributes["license"].Value = value;
                this.OnPropertyChanged("License");
            }
        }

        [Resources.ResourceProperty("nameof_Version", CC.ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Version
        {
            get
            {
                try { return this.Xml.Attributes["version"].Value; }
                catch (NullReferenceException) { return null; }
            }
            set
            {
                this.Xml.Attributes["version"].Value = value;
                this.OnPropertyChanged("Version");
            }
        }

        #region Alias

        public string nameof_Author
        {
            get { return Properties.Resources.Info_Author; }
        }

        public string nameof_License
        {
            get { return Properties.Resources.Info_License; }
        }

        public string nameof_Version
        {
            get { return Properties.Resources.Info_Version; }
        }

        #endregion

        internal new const string Alias = "library";
    }
}
