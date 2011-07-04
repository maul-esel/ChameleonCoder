using System;
using System.Windows.Controls;
using System.Windows.Media;
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

        public TaskResource() { }

        #region IResource

        public override string Alias { get { return "task"; } }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/task.png")); } }

        public override ImageSource TypeIcon { get { return this.Icon; } }

        #endregion

        /// <summary>
        /// opens the resource in the user interface
        /// </summary>
        public override void Open()
        {
            base.Open();

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("EndTime"), this.EndTime.ToLongDateString() }));

            //App.Gui.listView2.Groups[1].Header = Localization.get_string("info_task");
        }

        internal static void Create(object sender, EventArgs e)
        {

        }

        internal DateTime EndTime
        {
            get
            {
                try { return DateTime.Parse(this.XML.SelectSingleNode(this.XPath + "/@enddate").Value); }
                catch { return DateTime.Today; }
            }
            private set { this.XML.SelectSingleNode(this.XPath + "/@enddate").Value = value.ToString(); }
        }
    }
}
