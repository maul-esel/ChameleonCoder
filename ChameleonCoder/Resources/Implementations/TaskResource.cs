using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a task,
    /// inherits from ResourceBase
    /// </summary>
    public class TaskResource : ResourceBase
    {
        public TaskResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/task.png")); } }

        #endregion

        #region IEnumerable

        System.Collections.IEnumerator baseEnum;

        public override System.Collections.IEnumerator GetEnumerator()
        {
            this.baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            yield return new { Name = "end-time", Value = this.EndTime.ToString(), Group = "task" };
        }

        #endregion

        public DateTime EndTime
        {
            get
            {
                try { return DateTime.Parse(this.XMLNode.Attributes["enddate"].Value); }
                catch (ArgumentNullException) { return DateTime.MaxValue; }
                catch (FormatException) { return DateTime.MaxValue; }
                catch (NullReferenceException) { return DateTime.MaxValue; }
            }
            protected set
            {
                this.XMLNode.Attributes["enddate"].Value = value.ToString();
                this.OnPropertyChanged("EndTime");
            }
        }
    }
}
