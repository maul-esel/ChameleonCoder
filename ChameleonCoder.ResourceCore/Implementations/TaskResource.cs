using System;
using System.Windows.Media;
using System.Xml;
using System.Collections.Generic;
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
                try { return DateTime.Parse(this.Xml.Attributes["enddate"].Value); }
                catch (ArgumentNullException) { return DateTime.MaxValue; }
                catch (FormatException) { return DateTime.MaxValue; }
                catch (NullReferenceException) { return DateTime.MaxValue; }
            }
            protected set
            {
                this.Xml.Attributes["enddate"].Value = value.ToString();
                this.OnPropertyChanged("EndTime");
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

    }
}
