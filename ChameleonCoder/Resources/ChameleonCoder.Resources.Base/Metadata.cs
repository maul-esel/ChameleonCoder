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
        /// defines if this metadata can be used for configuration
        /// </summary>
        public bool IsNoConfig
        {
            get
            {
                try
                {
                    return this.XML.CreateNavigator().SelectSingleNode(this.XPath + "/@noconfig").ValueAsBoolean;
                }
                catch (NullReferenceException) { return false; }
            }
            private set
            {
                int i;
                if (value)
                    i = 1;
                else
                    i = 0;
                this.XML.SelectSingleNode(this.XPath + "/@noconfig").Value = i.ToString();
            }
        }

        /// <summary>
        /// defines if this metadata is used as default
        /// </summary>
        public bool IsDefault
        {
            get { return this.XML.CreateNavigator().SelectSingleNode(this.XPath + "/@default").ValueAsBoolean; }
            private set
            {
                int i;
                if (value)
                    i = 1;
                else
                    i = 0;
                this.XML.SelectSingleNode(this.XPath + "/@default").Value = i.ToString();
            }
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

        /// <summary>
        /// converts the instance to a SortedList
        /// </summary>
        /// <returns>the SortedList containing the instance</returns>
        internal SortedList ToSortedList()
        {
            SortedList list = new SortedList();

            list.Add("name", this.Name);
            list.Add("value", this.Value);
            list.Add("noconfig", this.IsNoConfig);
            list.Add("standard", this.IsDefault);

            return list;
        }
    }
}
