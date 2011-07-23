using System;
using System.Windows.Media;
using System.Xml;

namespace ChameleonCoder.Resources.Implementations
{
    public class GroupResource : ResourceBase
    {
        public GroupResource(XmlNode node, Interfaces.IResource parent)
            : base(node, parent)
        {
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")); } }
    }
}
