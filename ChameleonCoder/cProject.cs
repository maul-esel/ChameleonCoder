using System;
using System.Windows.Forms;
using System.Collections;
using System.Xml;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a project resource
    /// </summary>
    internal sealed class cProject : cResource
    {
        /// <summary>
        /// instantiates a new instance of the cProject class
        /// </summary>
        /// <param name="xmlnav">a XPathNavigator containing the resource document</param>
        /// <param name="xpath">the xpath to the resource's main element</param>
        /// <param name="datafile">the file that contains the definition</param>
        internal cProject(ref XmlDocument xmlnav, string xpath, string datafile)
            : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.project;

            this.Node.ImageIndex = 3;

            this.Language = new Guid(xmlnav.SelectSingleNode(xpath + "/@language").Value);

            try { this.Priority = (ProjectPriority)xmlnav.CreateNavigator().SelectSingleNode(xpath + "/@priority").ValueAsInt; }
            catch { this.Priority = ProjectPriority.basic; }
            this.Node.StateImageIndex = (int)this.Priority;

            try { this.CompilationPath = xmlnav.SelectSingleNode(xpath + "/@compilation-path").Value; }
            catch { }
        }

        /// <summary>
        /// defines a project's priority
        /// </summary>
        internal enum ProjectPriority
        {
            /// <summary>
            /// the project has the default (low) priority
            /// </summary>
            basic,

            /// <summary>
            /// the project has a slightly higher priority (middle)
            /// </summary>
            middle,

            /// <summary>
            /// the project has a high priority
            /// </summary>
            high
        }

        #region methods
        /// <summary>
        /// opens the project
        /// </summary>
        internal override void Open()
        {
            base.Open();

            ListViewItem item;
            
            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("Priority"), ToString(this.Priority) }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CodeLanguage"), Plugins.PluginManager.GetLanguageName(this.Language) }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            item = Program.Gui.listView2.Items.Add(new ListViewItem(new string[] { Localization.get_string("CompilePath"), this.CompilationPath }));
            Program.Gui.listView2.Groups[1].Items.Add(item);

            Program.Gui.listView2.Groups[1].Header = Localization.get_string("info_project");
        }

        #endregion

        internal override SortedList ToSortedList()
        {
            SortedList list = base.ToSortedList();

            list.Add("CompilationPath", this.CompilationPath);
            list.Add("Language", this.Language);
            list.Add("Priority", this.Priority);

            return list;
        }

        private static string ToString(ProjectPriority priority)
        {
            switch (priority)
            {
                case ProjectPriority.basic: return Localization.get_string("Priority_Basic");
                case ProjectPriority.middle: return Localization.get_string("Priority_Middle");
                case ProjectPriority.high: return Localization.get_string("Priority_High");
                default: return string.Empty;
            }
        }

        #region cProject properties

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        internal ProjectPriority Priority { get; private set; }

        /// <summary>
        /// the GUID of the language in which the project is written
        /// </summary>
        internal Guid Language { get; private set; }

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
