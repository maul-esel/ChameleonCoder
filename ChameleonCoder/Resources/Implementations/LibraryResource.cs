using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a library resource,
    /// inherits from CodeResource
    /// </summary>
    public sealed class LibraryResource : CodeResource
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

        internal string Author
        {
            get { return this.XMLNode.Attributes["author"].Value; }
            private set { this.XMLNode.Attributes["author"].Value = value; }
        }

        internal string License
        {
            get { return this.XMLNode.Attributes["license"].Value; }
            private set { this.XMLNode.Attributes["license"].Value = value; }
        }

        internal string Version
        {
            get { return this.XMLNode.Attributes["version"].Value; }
            private set { this.XMLNode.Attributes["version"].Value = value; }
        }
    }
}
