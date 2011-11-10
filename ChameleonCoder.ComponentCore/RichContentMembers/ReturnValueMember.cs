using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents the return value of a function or method
    /// </summary>
    public class ReturnValueMember : VariableMember
    {
        /// <summary>
        /// creates a new instance of the ReturnValueMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public ReturnValueMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        protected override string ElementName
        {
            get { return "ReturnValue"; }
        }

        protected override Plugins.Syntax.SyntaxElement Element
        {
            get { return Plugins.Syntax.SyntaxElement.ReturnValue; }
        }

        internal new const string Key = "{3cbd3e0a-a17c-4c04-866a-5f1bf6796727}";
    }
}
