using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

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
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/library.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        [ResourceProperty("nameof_Author", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Author
        {
            get
            {
                return Xml.GetAttribute("author");
            }
            set
            {
                Xml.SetAttribute("author", value);
                OnPropertyChanged("Author");
            }
        }

        [ResourceProperty("nameof_License", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string License
        {
            get
            {
                return Xml.GetAttribute("license");
            }
            set
            {
                Xml.SetAttribute("license", value);
                OnPropertyChanged("License");
            }
        }

        [Resources.ResourceProperty("nameof_Version", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Version
        {
            get
            {
                return Xml.GetAttribute("version");
            }
            set
            {
                Xml.SetAttribute("version", value);
                OnPropertyChanged("Version");
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
