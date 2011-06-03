﻿using System;
using System.Windows.Forms;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class cFile : IResource
    {
        public cFile(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Hide = xmlnav.SelectSingleNode(xpath + "/@hide").ValueAsBoolean;
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Type = ResourceType.file;
            this.XML = xmlnav;
            this.XPath = xpath;
            
            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 1;
            this.Item = new ListViewItem(new string[] { this.Name, this.Description });
            this.Path = xmlnav.SelectSingleNode(xpath + "/@path").Value;
        }

        #region IResource.properties

        public string DataFile { get; set; }

        public string Description { get; set; }
        
        public Guid GUID { get; set; }

        public bool Hide { get; set; }

        public ListViewItem Item { get; set; }

        public string Name { get; set; }

        public TreeNode Node { get; set; }

        public string Notes { get; set; }

        public Guid Parent { get; set; }

        public ResourceType Type { get; set; }

        public XPathNavigator XML { get; set; }

        public string XPath { get; set; }

        #endregion

        #region cFile properties

        /// <summary>
        /// the path to the file representedc by the resource
        /// </summary>
        public string Path { get; protected set; }

        #endregion

        #region methods
        void IResource.Move()
        {

        }

        void IResource.ReceiveResourceLink()
        {

        }

        void IResource.LinkResource()
        {

        }

        void IResource.ReceiveResource()
        {

        }

        void IResource.AttachResource()
        {

        }

        void IResource.SaveToFile()
        {

        }

        void IResource.SaveToObject()
        {

        }

        void IResource.Package()
        {

        }

        void IResource.OpenAsDescendant()
        {

        }

        void IResource.OpenAsAncestor()
        {

        }

        void IResource.AddMetadata()
        {

        }

        void IResource.Delete()
        {

        }
        #endregion
                       
    }
}
