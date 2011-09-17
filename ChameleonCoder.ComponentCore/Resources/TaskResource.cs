using System;
using System.Windows.Media;
using ChameleonCoder.Resources;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a task,
    /// inherits from ResourceBase
    /// </summary>
    public class TaskResource : ResourceBase
    {
        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/task.png")).GetAsFrozen() as ImageSource; } }

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

        [ResourceProperty("NameOfEndDate", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string EndDateName
        {
            get
            {
                return EndDate.ToLongDateString();
            }
        }

        public static string NameOfEndDate
        {
            get
            {
                return Properties.Resources.Info_EndDate;
            }
        }

        internal const string Alias = "task";
    }
}
