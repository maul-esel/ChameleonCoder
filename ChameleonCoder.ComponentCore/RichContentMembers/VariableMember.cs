using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    public class VariableMember : IContentMember
    {
        public RichContentCollection childMembers { get; protected set; }

        public string GetHtml(object param = null)
        {
            return string.Empty;
        }

        public virtual void Save() { }

        public virtual void Init(System.Xml.XmlElement node) { }
    }
}
