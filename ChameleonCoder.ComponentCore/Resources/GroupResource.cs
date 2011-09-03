using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a simple group of resources
    /// </summary>
    public class GroupResource : ResourceBase
    {
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/group.png")).GetAsFrozen() as ImageSource; } }

        internal const string Alias = "group";
    }
}
