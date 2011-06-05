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
            this.Node.ImageIndex = 0;
            this.Item = new ListViewItem(new string[] { this.Name, this.Description });
            this.Path = xmlnav.SelectSingleNode(xpath + "/@path").Value;
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

        void IResource.Package()
        {

        }

        void IResource.Open()
        {
            Program.Gui.listView2.Items.Clear();
            Program.Gui.dataGridView1.Rows.Clear();

            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Name"), this.Name }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("ResourceType"), HelperClass.ToString(this.Type) }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Tree"), "/" + this.Node.FullPath }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Description"), this.Description }));
            Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Path"), this.Path }));

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

        void IResource.AddMetadata()
        {

        }

        void IResource.Delete()
        {

        }
        #endregion

        #region cFile properties

        /// <summary>
        /// the path to the file represented by the resource
        /// </summary>
        internal string Path { get; set; }

        #endregion

        #region cFile methods

        internal static void Create(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            Program.Selector.Close();
        }

        #endregion
    }
}
