using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a parameter passed to a function or method
    /// </summary>
    public class ParameterMember : VariableMember
    {
        /// <summary>
        /// creates a new instance of the ParameterMember class
        /// </summary>
        /// <param name="data">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public ParameterMember(System.Xml.XmlElement data, IContentMember parent, IRichContentResource resource)
            : base(data, parent, resource)
        {
        }

        /// <summary>
        /// gets a parameter's default value
        /// </summary>
        public string DefaultValue
        {
            get { return Xml.GetAttribute("default-value", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("default-value", DataFile.NamespaceUri, value); }
        }

        /// <summary>
        /// gets if a parameter is byRef or not
        /// </summary>
        public string IsByRef
        {
            get { return Xml.GetAttribute("isbyref", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("isbyRef", DataFile.NamespaceUri, value); }
        }


        protected override string ElementName
        {
            get { return "Parameter"; }
        }

        protected override Plugins.Syntax.SyntaxElement Element
        {
            get { throw new System.NotSupportedException(); /*return Plugins.Syntax.SyntaxElement.Parameter;*/ }
        }

        internal new const string Key = "{d78144be-71b4-46f9-a947-4a5e3bf0caa9}";
    }
}
