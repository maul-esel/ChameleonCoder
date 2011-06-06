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
    internal sealed class cProject : cResource
    {
        internal cProject(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
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

        #region methods
        /// <summary>
        /// opens the project
        /// </summary>
        internal override void Open()
        {
            base.Open();

            ListViewItem item;
            
            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Priority"), HelperClass.ToString(this.Priority) }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CodeLanguage"), Plugins.PluginManager.GetLanguageName(this.language) }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CompilePath"), this.CompilationPath }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_project");
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
