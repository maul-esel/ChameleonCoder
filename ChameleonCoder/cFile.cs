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
        {
            this.DataFile = datafile;
            this.Description = xmlnav.SelectSingleNode(xpath + "/@description").Value;
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
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

        #region cResource methods

        internal override void Open()
        {
            base.Open();

            ListViewItem item;

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Path"), this.Path }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_file");
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
