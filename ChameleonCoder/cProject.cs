using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml.XPath;

namespace ChameleonCoder
{
    internal enum Priority
    {
        basic,
        middle,
        high
    }

    /// <summary>
    /// represents a project resource
    /// </summary>
    internal sealed class cProject : IResource
    {
        internal cProject(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Hide = xmlnav.SelectSingleNode(xpath + "/@hide").ValueAsBoolean;
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Type = ResourceType.project;
            this.XML = xmlnav;
            this.XPath = xpath;

            this.MetaData = new SortedList();
            //this.Flags = new MetaFlags[].;

            int i = 0;
            try
            {
                foreach (XPathNavigator xml in xmlnav.Select(xpath + "/metadata"))
                {
                    i++;
                    this.MetaData.Add(xml.SelectSingleNode(xpath + "/metadata[" + i + "]/@name").Value, xml.SelectSingleNode(xpath + "/metadata[" + i + "]").Value);
                    //this.MetaData[i] = MetaFlags.none;
                }
            }
            catch { }

            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 3;
            this.Item = new ListViewItem(new string[] { this.Name, this.Description });

            this.language = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);

            try { this.Priority = (Priority)xmlnav.SelectSingleNode(xpath + "/@priority").ValueAsInt; }
            catch { this.Priority = Priority.basic; }
            this.Node.StateImageIndex = (int)this.Priority;

            try { this.CompilationPath = xmlnav.SelectSingleNode(xpath + "/@compilation-path").Value; }
            catch { }
        }

        #region IResource properties

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

        #region IResource methods

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
        void IResource.Open()
        {
            Program.Gui.listView2.Items.Clear();
            Program.Gui.dataGridView1.Rows.Clear();

            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Name"), this.Name }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("ResourceType"), HelperClass.ToString(this.Type) }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Tree"), "/" + this.Node.FullPath }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Description"), this.Description }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Priority"), HelperClass.ToString(this.Priority) }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CodeLanguage"), Plugins.PluginManager.GetLanguageName(this.language) }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CompilePath"), this.CompilationPath }));

            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("DataFile"), this.DataFile }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("GUID"), this.GUID.ToString() }));

            Program.Gui.textBox1.Text = this.Notes;

            try
            {
                for (int i = 0; i <= this.MetaData.Count; i++)
                {
                    Program.Gui.dataGridView1.Rows.Add(new string[] { this.MetaData.GetKey(i).ToString(), this.MetaData.GetByIndex(i).ToString() });
                }
            }
            catch { }

            Program.Gui.listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            Program.Gui.panel1.Hide();
            Program.Gui.panel2.Hide();
            Program.Gui.panel3.Show();
        }

        #endregion

        #region cProject properties

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        internal Priority Priority { get; private set; }

        /// <summary>
        /// the GUID of the language in which the project is written
        /// </summary>
        internal Guid language { get; private set; }

        /// <summary>
        /// the path to which the project would be compiled
        /// </summary>
        internal string CompilationPath { get; private set; }

        #endregion

        #region cProject methods

        /// <summary>
        /// asks the user to enter a new priority and saves it
        /// </summary>
        internal void SetPriority()
        {

        }

        internal static void Create(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            Program.Selector.Close();
        }

        #endregion
               
    }
}
