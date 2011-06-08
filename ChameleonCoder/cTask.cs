using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a task
    /// </summary>
    internal sealed class cTask : cResource
    {
        internal cTask(ref XmlDocument xmlnav, string xpath, string datafile)
            : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.task;

            this.Node.ImageIndex = 4;

            try { this.EndTime = DateTime.Parse(xmlnav.SelectSingleNode(xpath + "/@enddate").Value); }
            catch { }
            this.EndTime = this.EndTime == DateTime.MinValue ? DateTime.Today : this.EndTime;
        }

        /// <summary>
        /// opens the resource in the user interface
        /// </summary>
        internal override void Open()
        {
            base.Open();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("EndTime"), this.EndTime.ToLongDateString() }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_task");
        }

        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("EndTime", this.EndTime);

            return list;
        }

        internal static void Create(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            Program.Selector.Close();
        }

        #region cTask properties

        internal DateTime EndTime;

        #endregion
    }
}
