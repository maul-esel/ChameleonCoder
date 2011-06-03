using System;
using System.Windows.Forms;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    /// <summary>
    /// represents a project resource
    /// </summary>
    public class cProject : IResource
    {
        public cProject(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Hide = xmlnav.SelectSingleNode(xpath + "/@hide").ValueAsBoolean;
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Type = ResourceType.project;
            this.XML = xmlnav;
            this.XPath = xpath;

            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 3;
            this.Item = new ListViewItem(new string[] { this.Name, this.Description });

            int i = 0;
            foreach (XPathNavigator node in xmlnav.Select(xpath + "/languages/lang"))
            {
                i++;
                languages[i] = Guid.Parse(node.SelectSingleNode("/@guid").Value);
            }
            this.Priority = xmlnav.SelectSingleNode(xpath + "/@priority").ValueAsInt;
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

        #region cProject properties

        /// <summary>
        /// contains the project's priority (int from 1 to 3)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// contains all languages to which the project is compatible
        /// </summary>
        public Guid[] languages;

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

        void IResource.OpenAsDescendant()
        {

        }

        void IResource.OpenAsAncestor()
        {

        }

        void IResource.AddMetadata()
        {

        }

        void IResource.Package()
        {

        }

        void IResource.Delete()
        {

        }

        /// <summary>
        /// opens the project
        /// </summary>
        public void Open()
        {

        }

        /// <summary>
        /// asks the user to enter a new priority and saves it
        /// </summary>
        public void SetPriority()
        {

        }

        #endregion
               
    }
}
