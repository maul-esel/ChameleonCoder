using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a field in a class
    /// </summary>
    public class FieldMember : VariableMember
    {
        public FieldMember(System.Xml.XmlElement node, IContentMember parent)
            : base(node, parent)
        {
        }

        internal new const string Key = "{b98b04ea-71ff-4550-a4c1-2a8bf4b9121c}";
    }
}
