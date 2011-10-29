using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a variable
    /// </summary>
    public class VariableMember : MemberBase
    {
        /// <summary>
        /// creates a new instance of the VariableMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public VariableMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        /// <summary>
        /// gets the member's HTML representation
        /// </summary>
        /// <param name="param">a parameter passed to the method, no special use</param>
        /// <returns>the representation as HTML text</returns>
        public override string GetHtml(object param)
        {
            return "<div class='builtin-container' id='" + Identifier.ToString("b") + "'><h3>Variable: " + Name + "</h3><p>[description]</p><pre class='builtin-example'>[example here] <em class='builtin-comment'> ; comment here</em></pre></div>";
        }

        internal const string Key = "{23f3716c-08a1-44d3-8a54-ac01c29435a2}";
    }
}
