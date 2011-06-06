using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml.XPath;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a task
    /// </summary>
    internal sealed class cTask : cResource
    {
        internal cTask(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            this.Notes = xmlnav.SelectSingleNode(xpath + "/@notes").Value;
            this.Type = ResourceType.task;
            this.XML = xmlnav;
            this.XPath = xpath;

            try { this.EndTime = DateTime.Parse(xmlnav.SelectSingleNode(xpath + "/@enddate").Value); }
            catch { }
            this.EndTime = this.EndTime == DateTime.MinValue ? DateTime.Today : this.EndTime;

            this.Node = new TreeNode(this.Name);
            this.Node.ImageIndex = 4;
            this.Item = new ListViewItem(new string[] { this.Name, this.Description });
        }

        internal override void Open()
        {
            base.Open();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("EndTime"), this.EndTime.ToLongDateString() }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_task");
        }

        #region cTask properties

        internal DateTime EndTime;

        #endregion

        #region cTask methods

        internal static void Create(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            Program.Selector.Close();
        }

        #endregion
    }
}
