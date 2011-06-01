using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AHKScriptsMan.Data
{
    /// <summary>
    /// represents a project resource
    /// </summary>
    public class cProject : cResource
    {
        #region properties
        /// <summary>
        /// contains the project's priority (int from 1 to 3)
        /// </summary>
        public int Priority
        {
            get;
            protected set;
        }

        /// <summary>
        /// contains a tree displayig the project's inheritance
        /// </summary>
        public string ProjectTree
        {
            get;
            protected set;
        }
        #endregion

        #region methods
       
        /// <summary>
        /// opens the project
        /// </summary>
        public override void Open()
        {

        }

        /// <summary>
        /// saves available information to the object
        /// </summary>
        public override void Save2Obj()
        {

        }

        /// <summary>
        /// saves the object's properties to the file
        /// </summary>
        public override void Save2File()
        {

        }

        /// <summary>
        /// asks the user to enter a new priority and saves it
        /// </summary>
        public void SetPriority()
        {

        }

        /// <summary>
        /// asks the user to enter name, flags and value of the new metadata
        /// </summary>
        public void AddMetadata()
        {

        }

        public override void Package()
        {
            
        }

        public cProject(XPathNavigator xmlnav, string xpath, IntPtr parentID, string datafile)
        {
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            if (ResourceList.HasKey(this.Name))
            {
                throw new Exception("duplicate resource name:" + this.Name + "\nresource type: project");
            }

            this.Priority = (int)xmlnav.SelectSingleNode(xpath + "/properties/priority").TypedValue;
            this.Type = ResourceType.project;
            this.ParentID = parentID;
            this.XML = xmlnav;
            this.XPath = xpath;
            this.DataFile = datafile;
            this.Node = new TreeNode(this.Name);
            ResourceList.Add((object)this);

            this.Node.ImageKey = "icon5";
            
            
        }
        #endregion
    }
}
