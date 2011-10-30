using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a field in a class
    /// </summary>
    public class FieldMember : VariableMember
    {
        /// <summary>
        /// creates a new instance of the FieldMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public FieldMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        protected override string ElementName
        {
            get { return "Field"; }
        }

        internal new const string Key = "{b98b04ea-71ff-4550-a4c1-2a8bf4b9121c}";
    }
}
