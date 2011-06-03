using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    /// <summary>
    /// represents a library resource
    /// </summary>
    public class cLibrary : IResource
    {
        public cLibrary(XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.Author = xmlnav.SelectSingleNode(xpath + "/@author").Value;
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Hide = xmlnav.SelectSingleNode(xpath + "/@hide").ValueAsBoolean;
            this.License = xmlnav.SelectSingleNode(xpath + "/@license").Value;
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Path = xmlnav.SelectSingleNode(xpath + "/@path").Value;
            this.Type = ResourceType.library;
            this.Version = xmlnav.SelectSingleNode(xpath + "/@version").Value;
            this.XML = xmlnav;
            this.XPath = xpath;

            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 2;
            this.Item = new ListViewItem(new string[] { this.Name, this.Type.ToString(), this.Description }); // add this.description (and this.typetostring())
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

        public ResourceType Type { get; set; }

        public XPathNavigator XML { get; set; }

        public string XPath { get; set; }

        #endregion

        #region cLibrary properties

        string Author { get; set; }

        string License { get; set; }

        string Version { get; set; }

        string Path { get; set; }

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
        #endregion
                
    }
}
