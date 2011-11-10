using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a constant value
    /// </summary>
    public class ConstantMember : VariableMember
    {
        /// <summary>
        /// creates a new instance of the ConstantMember class
        /// </summary>
        /// <param name="data">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public ConstantMember(System.Xml.XmlElement data, IContentMember parent, IRichContentResource resource)
            : base(data, parent, resource)
        {
        }

        /// <summary>
        /// gets the member's HTML representation
        /// </summary>
        /// <param name="param">not used.</param>
        /// <returns>the representation as HTML text</returns>
        public override string GetHtml(object param)
        {
            return base.GetHtml(param);
        }

        public string Value
        {
            get { return Xml.GetAttribute("value", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("value", DataFile.NamespaceUri, value); }
        }

        protected override string ElementName
        {
            get { return "Constant"; }
        }

        protected override Plugins.Syntax.SyntaxElement Element
        {
            get { return Plugins.Syntax.SyntaxElement.Constant; }
        }
    }
}
