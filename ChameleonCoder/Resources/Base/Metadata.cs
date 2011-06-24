using System;
using System.Collections;
using System.Xml;

namespace ChameleonCoder.Resources.Base
{
    /// <summary>
    /// represents a resource's metadata
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// contains the name of the metadata element
        /// </summary>
        public string Name
        {
            get { return this.XML.SelectSingleNode(this.XPath + "/@name").Value; }
            private set { this.XML.SelectSingleNode(this.XPath + "/@name").Value = value; }
        }

        /// <summary>
        /// contains the value of the metadata element
        /// </summary>
        public string Value
        {
            get { return this.XML.SelectSingleNode(this.XPath).InnerText; }
            private set { this.XML.SelectSingleNode(this.XPath).InnerText = value; }
        }

        /// <summary>
        /// contains the xpath to the element
        /// </summary>
        private string XPath { get; set; }

        /// <summary>
        /// the XmlDocument containing the definition of the metadata
        /// </summary>
        private XmlDocument XML;

        /// <summary>
        /// creates a new instance of the Metadata class
        /// </summary>
        /// <param name="xmlnav">the XmlDocument that contains the element</param>
        /// <param name="xpath">the xpath to the element</param>
        internal Metadata(ref XmlDocument xml, string xpath)
        {
            this.XPath = xpath;
            this.XML = xml;
        }

        /// <summary>
        /// saves any UI changes to the XmlDocument.
        /// Should better be handled through data binding.
        /// </summary>
        internal void Save()
        {

        }
    }
}
