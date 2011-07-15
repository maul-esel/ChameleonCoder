using System;
using System.Windows.Media;
using System.Xml;

namespace ChameleonCoder.Resources.Implementations
{
    public class GroupResource : ResourceBase, Interfaces.IAllowChildren
    {
        public GroupResource(XmlNode node)
            : base(node)
        {
            this.children = new Collections.ResourceCollection();
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")); } }

        public Collections.ResourceCollection children { get; protected set; }
    }
}
