using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace AHKScriptsMan.Data
{
    /// <summary>
    /// defines the resource's type
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// (this value is not used)
        /// </summary>
        resource,
        /// <summary>
        /// the resource is a file
        /// </summary>
        file,
        /// <summary>
        /// the resource is a library
        /// </summary>
        library,
        /// <summary>
        /// the resource is a project
        /// </summary>
        project,
        /// <summary>
        /// the resource is a task
        /// </summary>
        task

    }
    
    public abstract class cResource
    {
        #region properties
        /// <summary>
        /// contains the treeview id
        /// </summary>
        public TreeNode Node
        {
            get;
            protected set;
        }

        /// <summary>
        /// contains the user-defined name
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// contains the type of the resource
        /// </summary>
        public ResourceType Type
        {
            get;
            protected set;
        }

        /// <summary>
        /// contains the id of the parent item
        /// </summary>
        public IntPtr ParentID
        {
            get;
            protected set;
        }

        /// <summary>
        /// contains the xpath to the resource element
        /// </summary>
        public string XPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// contains the path to the data file of this resource
        /// </summary>
        public string DataFile
        {
            get;
            protected set;
        }

        public System.Xml.XPath.XPathNavigator XML
        {
            get;
            protected set;
        }
        // public compatibility
        #endregion

        #region methods
        /// <summary>
        /// redirects the "open"-request
        /// </summary>
        public virtual void Open()
        {
            if (ParentID != IntPtr.Zero)
                OpenProject();
            else
                OpenResource();
        }

        /// <summary>
        /// opens a resource inherited from a project
        /// </summary>
        protected virtual void OpenResource()
        {

        }

        /// <summary>
        /// opens a resource which isn't inherited
        /// </summary>
        protected virtual void OpenProject()
        {

        }

        /// <summary>
        /// packages a resource
        /// </summary>
        public virtual void Package()
        {

        }

        public virtual void Save2Obj()
        {

        }

        public virtual void Save2File()
        {

        }

        // maybe make this a constructor?
        public virtual void List(System.Xml.XPath.XPathNavigator xmlnav, string xpath, IntPtr parentID, string datafile)
        {
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            if (!ResourceList.HasKey(this.Name))
                {
                    this.Type = xmlnav.SelectSingleNode(xpath + "/@data-type").Value == "file" ? ResourceType.file : ResourceType.library;
                    this.ParentID = parentID;
                    this.XML = xmlnav;
                    this.XPath = xpath;
                    this.DataFile = datafile;
                    this.Node = new TreeNode(this.Name);
                    ResourceList.Add((object)this);
                }
            
           // LV_Add("", this.Name, TypeToString(this.Type), this.ID);
        }

        public virtual string TypeToString(ResourceType id)
        {
            switch (id)
            {
                case ResourceType.file: return "file";
                case ResourceType.library: return "library";
                case ResourceType.project: return "project";
                default: return "";
            }
        }
        #endregion

    }
}
