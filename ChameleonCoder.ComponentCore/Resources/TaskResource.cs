using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        /// <summary>
        /// gets the icon that represents this instance to the user
        /// </summary>
        /// <value>This is always the same as the TaskResource's type icon.</value>
        public override ImageSource Icon
        {
            get
            {
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/task.png"))
                    .GetAsFrozen() as ImageSource;
            }
        }

        #endregion

        /// <summary>
        /// gets the DateTime instance when the task will expire
        /// </summary>
        /// <value>The value is taken from the "enddate" attribute in the resource's XML.</value>
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

        /// <summary>
        /// gets the end date as a localized string
        /// </summary>
        [ResourceProperty("NameOfEndDate", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string EndDateName
        {
            get
            {
                return EndDate.ToLongDateString();
            }
        }

        /// <summary>
        /// gets the localized name of the <see cref="EndDate"/> property.
        /// </summary>
        /// <value>The value is taken from the localized resource file.</value>
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
