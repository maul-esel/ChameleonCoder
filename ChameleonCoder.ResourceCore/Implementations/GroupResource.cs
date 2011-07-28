using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    public class GroupResource : ResourceBase
    {
        public override void Init(XmlNode node, IResource parent)
        {
            base.Init(node, parent);
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/group.png")).GetAsFrozen() as ImageSource; } }
    }
}
