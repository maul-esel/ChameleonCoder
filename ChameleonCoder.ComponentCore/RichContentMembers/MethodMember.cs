using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a class or struct method
    /// </summary>
    public class MethodMember : FunctionMember
    {
        /// <summary>
        /// creates a new instance of the MethodMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public MethodMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        protected override string ElementName
        {
            get { return "Method"; }
        }

        protected override Plugins.Syntax.SyntaxElement Element
        {
            get { return Plugins.Syntax.SyntaxElement.Method; }
        }

        internal new const string Key = "{1a41bfb8-3b17-4290-b7a7-21da3f188eeb}";
    }
}
