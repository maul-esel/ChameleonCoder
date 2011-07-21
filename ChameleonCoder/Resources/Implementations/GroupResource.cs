using System;
using System.Windows.Media;
using System.Xml;

namespace ChameleonCoder.Resources.Implementations
{
    public class GroupResource : ResourceBase, Interfaces.IAllowChildren
    {
        public GroupResource(XmlNode node, Interfaces.IAllowChildren parent)
            : base(node, parent)
        {
            this.children = new ResourceCollection();
        }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")); } }

        public ResourceCollection children { get; protected set; }
    }
}
