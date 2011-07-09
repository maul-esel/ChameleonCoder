using System;
using System.Windows.Media;
using System.Xml;

namespace ChameleonCoder.Resources.Implementations
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
        public LibraryResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/library.png")); } }

        #endregion

        public string Author
        {
            get { return this.XMLNode.Attributes["author"].Value; }
            protected set { this.XMLNode.Attributes["author"].Value = value; }
        }

        public string License
        {
            get { return this.XMLNode.Attributes["license"].Value; }
            protected set { this.XMLNode.Attributes["license"].Value = value; }
        }

        public string Version
        {
            get { return this.XMLNode.Attributes["version"].Value; }
            protected set { this.XMLNode.Attributes["version"].Value = value; }
        }
    }
}
