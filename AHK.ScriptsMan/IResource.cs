using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AHKScriptsMan
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
        /// a file resource specified on code files
        /// </summary>
        code,
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
    
    public interface IResource
    {
        #region properties

        /// <summary>
        /// contains the path to the data file of this resource
        /// </summary>
        string DataFile { get; set; }

        /// <summary>
        /// a short description of the resource
        /// [optional]
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// contains a globally unique identifier.
        /// </summary>
        Guid GUID { get; set; }

        /// <summary>
        /// defines whether the resource is hidden or not
        /// </summary>
        bool Hide { get; set; }

        /// <summary>
        /// contains the listview item
        /// </summary>
        ListViewItem Item { get; set; }

        /// <summary>
        /// contains the user-defined name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// contains the treeview node
        /// </summary>
        TreeNode Node { get; set; }

        /// <summary>
        /// any notes related to the resource
        /// </summary>
        string Notes { get; set; }

        /// <summary>
        /// contains the parent's GUID
        /// </summary>
        Guid Parent { get; set; }

        /// <summary>
        /// contains the type of the resource
        /// </summary>
        ResourceType Type { get; set; }

        /// <summary>
        /// can be used to naviagte through the XML definition
        /// </summary>
        XPathNavigator XML { get; set; }

        /// <summary>
        /// contains the xpath to the resource element
        /// </summary>
        string XPath { get; set; }
                
        #endregion

        #region methods
        /// <summary>
        /// opens a resource inherited from a project
        /// </summary>
        void OpenAsAncestor();

        /// <summary>
        /// opens a resource which isn't inherited
        /// </summary>
        void OpenAsDescendant();

        /// <summary>
        /// packages a resource
        /// </summary>
        void Package();

        void SaveToObject();

        void SaveToFile();

        void AddMetadata();

        /// <summary>
        /// attaches a copy of the resource to another resource
        /// </summary>
        void AttachResource();

        /// <summary>
        /// receives a resource that should be attached
        /// </summary>
        void ReceiveResource();

        /// <summary>
        /// links a resource to another resource
        /// </summary>
        void LinkResource();

        /// <summary>
        /// receives a resource link
        /// </summary>
        void ReceiveResourceLink();

        /// <summary>
        /// moves a resource to another resource
        /// </summary>
        void Move(); 
            // first: attach
            // then: delete

        void Delete();

        #endregion

    }
}
