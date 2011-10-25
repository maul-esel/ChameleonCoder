using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents the return value of a function or method
    /// </summary>
    public class ReturnValueMember : VariableMember
    {
        public ReturnValueMember(System.Xml.XmlElement node, IContentMember parent)
            : base(node, parent)
        {
        }

        internal new const string Key = "{3cbd3e0a-a17c-4c04-866a-5f1bf6796727}";
    }
}
