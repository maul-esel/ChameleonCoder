using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    public class GroupResource : ResourceBase
    {
        public GroupResource(XmlNode node, IResource parent)
            : base(node, parent)
        {
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")); } }
    }
}
