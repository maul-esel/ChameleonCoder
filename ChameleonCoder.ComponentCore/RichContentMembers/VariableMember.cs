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
        /// gets a variable's type
        /// </summary>
        public string Type
        {
            get { return Xml.GetAttribute("var-type", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("var-type", DataFile.NamespaceUri, value); }
        }

        /// <summary>
        /// gets if a variable is readonly or not
        /// </summary>
        public bool IsReadOnly
        {
            get { return bool.Parse(Xml.GetAttribute("isreadonly", DataFile.NamespaceUri)); }
            protected set { Xml.SetAttribute("isreadonly", DataFile.NamespaceUri, value.ToString()); }
        }


        protected override string ElementName
        {
            get { return "Variable"; }
        }

        protected override Plugins.Syntax.SyntaxElement Element
        {
            get { return Plugins.Syntax.SyntaxElement.Variable; }
        }

        internal const string Key = "{23f3716c-08a1-44d3-8a54-ac01c29435a2}";
    }
}
