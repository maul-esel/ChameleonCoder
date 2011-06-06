using System;
using System.Collections;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace ChameleonCoder
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

    [FlagsAttribute]
    public enum MetaFlags
    {
        none = 0,
        hide = 1,
        noconfig = 2,
        noedit = 4,
        standard = 8, 
    }
    
    internal abstract class cResource
    {
        #region properties

        /// <summary>
        /// contains the path to the data file of this resource
        /// </summary>
        public string DataFile { get; protected internal set; }

        /// <summary>
        /// a short description of the resource
        /// [optional]
        /// </summary>
        public string Description { get; protected internal set; }

        /// <summary>
        /// contains a globally unique identifier.
        /// </summary>
        public Guid GUID { get; protected internal set; }

        /// <summary>
        /// contains the listview item
        /// </summary>
        public ListViewItem Item { get; protected internal set; }

        /// <summary>
        /// the associated metadata as key-value pairs
        /// </summary>
        public SortedList MetaData { get; protected internal set; }

        /// <summary>
        /// the flags associated to the metadata
        /// </summary>
        public MetaFlags[] Flags { get; protected internal set; } // maybe: requires int[] ?

        /// <summary>
        /// contains the user-defined name
        /// </summary>
        public string Name { get; protected internal set; }

        /// <summary>
        /// contains the treeview node
        /// </summary>
        public TreeNode Node { get; protected internal set; }

        /// <summary>
        /// any notes related to the resource
        /// </summary>
        public string Notes { get; protected internal set; }

        /// <summary>
        /// contains the type of the resource
        /// </summary>
        public ResourceType Type { get; protected internal set; }

        /// <summary>
        /// can be used to naviagte through the XML definition
        /// </summary>
        public XPathNavigator XML { get; protected internal set; }

        /// <summary>
        /// contains the xpath to the resource element
        /// </summary>
        public string XPath { get; protected internal set; }
                
        #endregion

        #region methods
        /// <summary>
        /// opens a resource
        /// </summary>
        internal virtual void Open()
        {
            Program.Gui.listView2.Items.Clear();
            Program.Gui.dataGridView1.Rows.Clear();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Name"), this.Name }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("ResourceType"), HelperClass.ToString(this.Type) }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Tree"), "/" + this.Node.FullPath }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Description"), this.Description }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("DataFile"), this.DataFile }));
            Program.Gui.listView2.Groups[2].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("GUID"), this.GUID.ToString() }));
            Program.Gui.listView2.Groups[2].Items.Add(item);

            Program.Gui.textBox1.Text = this.Notes;

            try
            {
                for (int i = 0; i <= this.MetaData.Count; i++)
                {
                    Program.Gui.dataGridView1.Rows.Add(new string[] { this.MetaData.GetKey(i).ToString(), this.MetaData.GetByIndex(i).ToString() });
                }
            }
            catch { }
        }

        /// <summary>
        /// packages a resource
        /// </summary>
        internal virtual void Package()
        {

        }

        /// <summary>
        /// saves the information changed through the UI to the current instance
        /// </summary>
        internal virtual void SaveToObject()
        {

        }

        /// <summary>
        /// saves the information stored in the instance to the XML property
        /// and then to the file
        /// </summary>
        internal virtual void SaveToFile()
        {

        }

        /// <summary>
        /// adds a metadata element
        /// </summary>
        internal virtual void AddMetadata()
        {

        }

        /// <summary>
        /// attaches a copy of the resource to another resource
        /// </summary>
        internal virtual void AttachResource() // todo: parameter to define where the guid goes
        {

        }

        /// <summary>
        /// receives a resource that should be attached
        /// </summary>
        internal virtual void ReceiveResource()
        {

        }

        /// <summary>
        /// links a resource to another resource
        /// </summary>
        internal virtual void LinkResource()
        {

        }

        /// <summary>
        /// receives a resource link
        /// </summary>
        internal virtual void ReceiveResourceLink()
        {

        }

        /// <summary>
        /// moves a resource to another resource
        /// </summary>
        internal virtual void Move()
        {

        }
            // first: attach
            // then: delete

        internal virtual void Delete()
        {
            ResourceList.Remove(this.Node.GetHashCode());
            this.Node.Remove();
        }

        #endregion
    }
}
