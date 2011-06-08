using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a library resource
    /// </summary>
    internal sealed class cLibrary : cCodeFile
    {
        internal cLibrary(ref XmlDocument xmlnav, string xpath, string datafile)
            : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.library;

            this.Node.ImageIndex = 2;

            try { this.Author = xmlnav.SelectSingleNode(xpath + "/@author").Value; }
            catch { }

            try { this.License = xmlnav.SelectSingleNode(xpath + "/@license").Value; }
            catch { }

            try { this.Version = xmlnav.SelectSingleNode(xpath + "/@version").Value; }
            catch { }
        }

        internal override void Open()
        {
            base.Open();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Author"), this.Author }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("License"), this.License }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Version"), this.Version }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_lib");
        }

        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("Author", this.Author);
            list.Add("License", this.License);
            list.Add("Version", this.Version);

            return list;
        }

        #region cLibrary properties

        internal string Author { get; private set; }

        internal string License { get; private set; }

        internal string Version { get; private set; }

        #endregion

        #region cLibrary methods

        internal static new void Create(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            Program.Selector.Close();
        }

        #endregion
    }
}
