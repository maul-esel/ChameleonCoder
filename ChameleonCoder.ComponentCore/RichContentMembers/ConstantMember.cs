using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    public class ConstantMember : VariableMember
    {
        public ConstantMember(System.Xml.XmlElement data, IContentMember parent, IRichContentResource resource)
            : base(data, parent, resource)
        {
        }

        public override string GetHtml(object param)
        {
            return base.GetHtml(param);
        }

        public string Value
        {
            get { return Xml.GetAttribute("value", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("value", DataFile.NamespaceUri, value); }
        }
    }
}
