using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    internal class FileResource : ResourceBase
    {
        internal FileResource(ref XmlDocument xml, string xpath, string datafile)
            : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.file;
        }

        #region properties

        /// <summary>
        /// the path to the file represented by the resource
        /// </summary>
        public string Path
        {
            get { return this.XML.SelectSingleNode(this.XPath + "/@path").Value; }
            protected set { this.XML.SelectSingleNode(this.XPath + "/@path").Value = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// opens the resource
        /// </summary>
        internal override void Open()
        {
            base.Open(); // as well as the base method, this currently doesn't work and should be done with data binding

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Path"), this.Path }));

            //App.Gui.listView2.Groups[1].Header = Localization.get_string("info_file");
        }

        /// <summary>
        /// compiles the instance into a SortedList which can be given to plugins
        /// </summary>
        /// <returns>the SortedList representing the resource</returns>
        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("Path", this.Path);

            return list;
        }

        #endregion

        /// <summary>
        /// creates a new FileResource
        /// </summary>
        internal static void Create()
        {

        }
    }
}
