using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a function
    /// </summary>
    public class FunctionMember : MemberBase
    {
        /// <summary>
        /// creates a new instance of the FunctionMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public FunctionMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        #region IContentMember

        /// <summary>
        /// gets the member's HTML representation
        /// </summary>
        /// <param name="param">a parameter passed to the method, no special use</param>
        /// <returns>the representation as HTML text</returns>
        public override string GetHtml(object param)
        {
            return string.Empty;
        }

        #endregion

        internal const string Key = "{2fc4ddba-0af1-474b-8af7-3154103fa77e}";
    }
}
