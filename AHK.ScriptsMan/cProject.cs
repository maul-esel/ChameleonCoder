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
        public cProject(XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.Compatible_AHKB = xmlnav.SelectSingleNode(xpath + "/@compatible_AHKB").ValueAsBoolean;
            this.Compatible_AHKL = xmlnav.SelectSingleNode(xpath + "/@compatible_AHKL").ValueAsBoolean;
            this.Compatible_AHKI = xmlnav.SelectSingleNode(xpath + "/@compatible_AHKI").ValueAsBoolean;
            this.Compatible_AHK2 = xmlnav.SelectSingleNode(xpath + "/@compatible_AHK2").ValueAsBoolean;            
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Hide = xmlnav.SelectSingleNode(xpath + "/@hide").ValueAsBoolean;
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Priority = xmlnav.SelectSingleNode(xpath + "/@priority").ValueAsInt;
            this.Type = ResourceType.project;
            this.XML = xmlnav;
            this.XPath = xpath;

            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 3;
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

        #region cProject properties

        /// <summary>
        /// contains the project's priority (int from 1 to 3)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// defines whether the project is compatible to AutoHotkey (basic).
        /// </summary>
        bool Compatible_AHKB { get; set; }

        /// <summary>
        /// defines whether the project is compatible to AutoHotkey_L.
        /// </summary>
        bool Compatible_AHKL { get; set; }

        /// <summary>
        /// defines whether the project is compatible to IronAHK.
        /// </summary>
        bool Compatible_AHKI { get; set; }

        /// <summary>
        /// defines whether the project is compatible to AutoHotkey v2.
        /// </summary>
        bool Compatible_AHK2 { get; set; }

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
