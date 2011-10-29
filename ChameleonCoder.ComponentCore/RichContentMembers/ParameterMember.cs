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
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public ParameterMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        /// <summary>
        /// gets a parameter's type
        /// </summary>
        public string Type
        {
            get { return Xml.GetAttribute("param-type", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("param-type", DataFile.NamespaceUri, value); }
        }

        /// <summary>
        /// gets a parameter's default value
        /// </summary>
        public string DefaultValue
        {
            get { return Xml.GetAttribute("default-value", DataFile.NamespaceUri); }
            protected set { Xml.SetAttribute("default-value", DataFile.NamespaceUri, value); }
        }

        internal new const string Key = "{d78144be-71b4-46f9-a947-4a5e3bf0caa9}";
    }
}
