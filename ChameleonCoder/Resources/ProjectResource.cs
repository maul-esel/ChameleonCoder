using System;
using System.Collections;
using System.Windows.Controls;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a project resource,
    /// inherits from ResourceBase
    /// </summary>
    public sealed class ProjectResource : ResourceBase
    {
        /// <summary>
        /// instantiates a new instance of the ProjectResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the xpath to the resource's main element</param>
        /// <param name="datafile">the file that contains the definition</param>
        internal ProjectResource(ref XmlDocument xml, string xpath, string datafile)
            : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.project;
            //this.Node.StateImageIndex = (int)this.Priority;
        }

        #region properties

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        internal ProjectPriority Priority
        {
            get { return (ProjectPriority)Int32.Parse(this.XML.SelectSingleNode(this.XPath + "/@priority").Value); }
            private set { this.XML.SelectSingleNode(this.XPath + "/@priority").Value = ((int)value).ToString(); }
        }

        /// <summary>
        /// the GUID of the language in which the project is written
        /// </summary>
        internal Guid Language
        {
            get { return new Guid(this.XML.SelectSingleNode(this.XPath + "/@language").Value); }
            private set { this.XML.SelectSingleNode(this.XPath + "/@language").Value = value.ToString(); }
        }

        /// <summary>
        /// the path to which the project would be compiled
        /// </summary>
        internal string CompilationPath
        {
            get { return this.XML.SelectSingleNode(this.XPath + "/@compilation-path").Value; }
            private set { this.XML.SelectSingleNode(this.XPath + "/@compilation-path").Value = value; }
        }

        #endregion

        /// <summary>
        /// defines a project's priority
        /// </summary>
        public enum ProjectPriority
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

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Priority"), ToString(this.Priority) }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("CodeLanguage"), Plugins.PluginManager.GetLanguageName(this.Language) }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("CompilePath"), this.CompilationPath }));

            //App.Gui.listView2.Groups[1].Header = Localization.get_string("info_project");
        }

        #endregion

        /// <summary>
        /// asks the user to enter a new priority and saves it
        /// </summary>
        internal void SetPriority()
        {

        }

        internal static void Create(object sender, EventArgs e)
        {

        }              
    }
}
