using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a library resource,
    /// inherits from CodeResource
    /// </summary>
    public sealed class LibraryResource : CodeResource
    {
        /// <summary>
        /// creates a new instance of the LibraryResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        internal LibraryResource(ref XmlDocument xml, string xpath, string datafile)
            : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.library;
        }

        public LibraryResource() { }

        #region IResource

        public override string Alias { get {  return "library"; } }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/library.png")); } }

        public override ImageSource TypeIcon { get { return this.Icon; } }

        #endregion

        internal string Author
        {
            get { return this.XML.SelectSingleNode(this.XPath + "/@author").Value; }
            private set { this.XML.SelectSingleNode(this.XPath + "/@author").Value = value; }
        }

        internal string License
        {
            get { return this.XML.SelectSingleNode(this.XPath + "/@license").Value; }
            private set { this.XML.SelectSingleNode(this.XPath + "/@license").Value = value; }
        }

        internal string Version
        {
            get { return this.XML.SelectSingleNode(this.XPath + "/@version").Value; }
            private set { this.XML.SelectSingleNode(this.XPath + "/@version").Value = value; }
        }

        /// <summary>
        /// opens the resource, using the CodeResource method + adding special information
        /// </summary>
        public override void Open()
        {
            base.Open(); // as well as the base method, this currently doesn't work and should be done with data binding

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Author"), this.Author }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("License"), this.License }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Version"), this.Version }));

            //App.Gui.listView2.Groups[1].Header = Localization.get_string("info_lib");
        }

        internal static void Create(object sender, EventArgs e)
        {
            App.Gui.IsEnabled = true;
            //App.Selector.Close();
        }
    }
}
