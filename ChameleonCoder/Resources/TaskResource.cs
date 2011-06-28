using System;
using System.Collections;
using System.Windows.Controls;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a task,
    /// inherits from ResourceBase
    /// </summary>
    public sealed class TaskResource : ResourceBase
    {
        internal TaskResource(ref XmlDocument xml, string xpath, string datafile)
            : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.task;
        }

        /// <summary>
        /// opens the resource in the user interface
        /// </summary>
        internal override void Open()
        {
            base.Open();

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("EndTime"), this.EndTime.ToLongDateString() }));

            //App.Gui.listView2.Groups[1].Header = Localization.get_string("info_task");
        }

        internal static void Create(object sender, EventArgs e)
        {

        }

        #region properties

        public override string Alias { get { return "task"; } }

        internal DateTime EndTime
        {
            get
            {
                try { return DateTime.Parse(this.XML.SelectSingleNode(this.XPath + "/@enddate").Value); }
                catch { return DateTime.Today; }
            }
            private set { this.XML.SelectSingleNode(this.XPath + "/@enddate").Value = value.ToString(); }
        }

        #endregion
    }
}
