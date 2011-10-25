using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represnts a struct or class property
    /// </summary>
    public class PropertyMember : VariableMember
    {
        public PropertyMember(System.Xml.XmlElement node, IContentMember parent)
            : base(node, parent)
        {
        }

        internal new const string Key = "{419bc576-0817-4b7a-8127-f5ad4f7863e7}";
    }
}
