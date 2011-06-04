using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml.XPath;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class cFile : IResource
    {
        internal cFile(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Hide = xmlnav.SelectSingleNode(xpath + "/@hide").ValueAsBoolean;
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Type = ResourceType.file;
            this.XML = xmlnav;
            this.XPath = xpath;
            
            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 0;
            this.Item = new ListViewItem(new string[] { this.Name, this.Description });
            this.Path = xmlnav.SelectSingleNode(xpath + "/@path").Value;
        }

        #region IResource.properties

        public string DataFile { get; set; }

        public string Description { get; set; }
        
        public Guid GUID { get; set; }

        public bool Hide { get; set; }

        public ListViewItem Item { get; set; }

        public SortedList MetaData { get; set; }

        public MetaFlags[] Flags { get; set; }

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
        /// the path to the file represented by the resource
        /// </summary>
        internal string Path { get; set; }

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
            Program.Gui.panel1.Hide();
            Program.Gui.panel2.Hide();
            Program.Gui.panel3.Show();
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
