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
    public class cLibrary : cCodeFile
    {
        public cLibrary(XPathNavigator xmlnav, string xpath, string datafile) : base(xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.library;
            
            this.Author = xmlnav.SelectSingleNode(xpath + "/@author").Value;
            this.License = xmlnav.SelectSingleNode(xpath + "/@license").Value;
            this.Version = xmlnav.SelectSingleNode(xpath + "/@version").Value;
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

        public string Author { get; protected set; }

        public string License { get; protected set; }

        public string Version { get; protected set; }

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
