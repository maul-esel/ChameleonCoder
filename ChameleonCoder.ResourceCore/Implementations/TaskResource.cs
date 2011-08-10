using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// represents a task,
    /// inherits from ResourceBase
    /// </summary>
    public class TaskResource : ResourceBase
    {
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/task.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        public DateTime EndDate
        {
            get
            {
                string value = Xml.GetAttribute("enddate");
                DateTime date;
                if (string.IsNullOrWhiteSpace(value) || !DateTime.TryParse(value, out date))
                    return DateTime.MaxValue;
                return date;
            }
            protected set
            {
                Xml.SetAttribute("enddate", value.ToString());
                OnPropertyChanged("EndDate");
            }
        }

        [ResourceProperty("nameof_EndDateName", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string EndDateName
        {
            get
            {
                return EndDate.ToLongDateString();
            }
        }

        public string nameof_EndDateName
        {
            get
            {
                return Properties.Resources.Info_EndDate;
            }
        }

        internal const string Alias = "task";
    }
}
