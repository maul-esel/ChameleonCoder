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
    public class FileResource : ResourceBase
    {
        internal FileResource(ref XmlDocument xml, string xpath, string datafile)
            : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.file;
        }

        public FileResource() { }

        #region IResource

        public override string Alias { get { return "file"; } }

        #endregion

        /// <summary>
        /// the path to the file represented by the resource
        /// </summary>
        public string Path
        {
            get
            {
                string path = string.Empty;
                try { path = this.XML.SelectSingleNode(this.XPath + "/@path").Value; }
                catch (NullReferenceException) { }
                if (!System.IO.Path.IsPathRooted(path) && path != string.Empty
                    && System.IO.Path.IsPathRooted(ChameleonCoder.Properties.Settings.Default.ScriptDir + path))
                    return ChameleonCoder.Properties.Settings.Default.ScriptDir + path;
                return path;
            }
            protected internal set { this.XML.SelectSingleNode(this.XPath + "/@path").Value = value; }
        }

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

        #endregion

        /// <summary>
        /// creates a new FileResource
        /// </summary>
        internal static void Create()
        {

        }
    }
}
