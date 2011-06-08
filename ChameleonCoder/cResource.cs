using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// defines the resource's type
    /// </summary>
    internal enum ResourceType
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

    internal abstract class cResource
    {
        protected cResource(ref XmlDocument xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.MetaData = new ArrayList();
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Type = ResourceType.resource;
            this.XML = xmlnav;
            this.XPath = xpath;

            int i = 0;
            Metadata data;
            string defaultdata = string.Empty;
            try
            {
                foreach (XmlNode xml in xmlnav.SelectNodes(xpath + "/metadata"))
                {
                    i++;
                    this.MetaData.Add(data = new Metadata(xmlnav, xpath + "/metadata[" + i + "]"));
                    if (data.IsDefault())
                    {
                        this.DefaultData = data;
                        defaultdata = data.GetName() + ": " + data.GetValue();
                    }
                }
            }
            catch { }

            this.Node = new TreeNode(this.Name);

            this.Item = new ListViewItem(new string[] { this.Name, this.Description, defaultdata  });
        }

        #region contained types

        /// <summary>
        /// represents a resource's metadata
        /// </summary>
        protected class Metadata
        {
            /// <summary>
            /// defines if this metadata can be used for configuration
            /// </summary>
            private bool noconfig;

            /// <summary>
            /// defines if this metadata is used as default
            /// </summary>
            private bool isdefault;

            /// <summary>
            /// contains the name of the metadata element
            /// </summary>
            private string name;

            /// <summary>
            /// contains the value of the metadata element
            /// </summary>
            private string value;

            /// <summary>
            /// contains the xpath to the element
            /// </summary>
            private string xpath;

            /// <summary>
            /// creates a new instance of the Metadata class
            /// </summary>
            /// <param name="xmlnav">the XPathNavigator that contains the element</param>
            /// <param name="xpath">the xpath to the element</param>
            internal Metadata(XmlDocument xmlnav, string xpath)
            {
                try { this.name = xmlnav.SelectSingleNode(xpath + "/@name").Value; }
                catch { } // output error!

                try { this.value = xmlnav.CreateNavigator().SelectSingleNode(xpath).Value; }
                catch { }

                try { this.noconfig = xmlnav.CreateNavigator().SelectSingleNode(xpath + "/@noconfig").ValueAsBoolean; }
                catch { }

                try { this.isdefault = xmlnav.CreateNavigator().SelectSingleNode(xpath + "/@default").ValueAsBoolean; }
                catch { }

                this.xpath = xpath;
            }

            /// <summary>
            /// gets whether the metadata is allowed to be used for configuration or not
            /// </summary>
            /// <returns>a boolean</returns>
            internal bool IsConfigAllowed()
            {
                return !this.noconfig;
            }

            /// <summary>
            /// gets whether the metadata is the standard metadata
            /// </summary>
            /// <returns></returns>
            internal bool IsDefault()
            {
                return this.isdefault;
            }

            /// <summary>
            /// gets the metadata's name
            /// </summary>
            /// <returns>the name (as string)</returns>
            internal string GetName()
            {
                return this.name;
            }

            /// <summary>
            /// gets the metadata's value
            /// </summary>
            /// <returns>the value (as string)</returns>
            internal string GetValue()
            {
                return this.value;
            }

            /// <summary>
            /// converts the instance to a SortedList
            /// </summary>
            /// <returns>the SortedList containing the instance</returns>
            internal SortedList ToSortedList()
            {
                SortedList list = new SortedList();

                list.Add("name", this.name);
                list.Add("value", this.value);
                list.Add("noconfig", this.noconfig);
                list.Add("standard", this.isdefault);

                return list;
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// contains the path to the data file of this resource
        /// </summary>
        public string DataFile { get; protected internal set; }

        /// <summary>
        /// contains the metadata marked as default
        /// </summary>
        protected Metadata DefaultData { get; set; }

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
        /// the associated metadata as Metadata class instances
        /// </summary>
        public ArrayList MetaData { get; protected internal set; }

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
        public XmlDocument XML { get; protected internal set; }

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

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("ResourceType"), ToString(this.Type) }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Tree"), "/" + this.Node.FullPath }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Description"), this.Description }));
            Program.Gui.listView2.Groups[0].Items.Add(item);

            try
            {
                item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { this.DefaultData.GetName(), this.DefaultData.GetValue() }));
                Program.Gui.listView2.Groups[0].Items.Add(item);
            }
            catch { }

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("DataFile"), this.DataFile }));
            Program.Gui.listView2.Groups[2].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("GUID"), this.GUID.ToString() }));
            Program.Gui.listView2.Groups[2].Items.Add(item);

            Program.Gui.textBox1.Text = this.Notes;

            try
            {
                foreach (Metadata data in this.MetaData)
                {
                    Program.Gui.dataGridView1.Rows.Add(new string[] { data.GetName(), data.GetValue() });

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
        /// saves the information changed through the UI to the current instance and its XML representation
        /// </summary>
        internal virtual void Save()
        {
            try
            {
                this.XML.SelectSingleNode(this.XPath + "/@notes").Value = this.Notes = Program.Gui.textBox1.Text;

                this.XML.SelectSingleNode(this.XPath + "/@description").Value = this.Description;
                this.XML.SelectSingleNode(this.XPath + "/@name").Value = this.Name;
                this.XML.SelectSingleNode(this.XPath + "/@type").Value = ((int)this.Type).ToString();
            }
            catch { }
            File.WriteAllText(this.DataFile, this.XML.DocumentElement.OuterXml);
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
        internal virtual void ReceiveAttach()
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
        internal virtual void ReceiveLink()
        {

        }

        /// <summary>
        /// moves a resource to another resource
        /// </summary>
        internal virtual void Move()
        {

        }
            // first: attach
            // then: delete --> option (param) who get's the GUID

        /// <summary>
        /// deletes a resource
        /// </summary>
        internal virtual void Delete()
        {
            ResourceList.Remove(this.Node.GetHashCode());
            this.Node.Remove();
        }

        /// <summary>
        /// compiles a resource object into a SortedList which can be given to plugins
        /// </summary>
        /// <returns></returns>
        internal virtual SortedList ToSortedList()
        {
            SortedList list = new SortedList();

            list.Add("Description", this.Description);
            list.Add("GUID", this.GUID);
            list.Add("MetaData", this.MetaData);
            list.Add("Name", this.Name);
            list.Add("Notes", this.Notes);
            list.Add("Type", this.Type);
            list.Add("XML", this.XML);
            list.Add("XPath", this.XPath);

            return list;
        }

        #endregion

        protected static string ToString(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.resource: return Localization.get_string("ResourceType_Resource");
                case ResourceType.file: return Localization.get_string("ResourceType_File");
                case ResourceType.code: return Localization.get_string("ResourceType_Code");
                case ResourceType.library: return Localization.get_string("ResourceType_Library");
                case ResourceType.project: return Localization.get_string("ResourceType_Project");
                case ResourceType.task: return Localization.get_string("ResourceType_Task");
                default: return string.Empty;
            }
        }
    }
}
