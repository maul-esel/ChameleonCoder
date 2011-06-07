using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml.XPath;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    internal class cFile : cResource
    {
        internal cFile(ref XPathNavigator xmlnav, string xpath, string datafile)
            : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.file;

            this.Node.ImageIndex = 0;

            this.Path = xmlnav.SelectSingleNode(xpath + "/@path").Value;
        }

        #region cResource methods

        internal override void Open()
        {
            base.Open();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Path"), this.Path }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_file");
        }

        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("Path", this.Path);

            return list;
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
