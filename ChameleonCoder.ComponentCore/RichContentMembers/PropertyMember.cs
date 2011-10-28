using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a struct or class property
    /// </summary>
    public class PropertyMember : VariableMember
    {
        /// <summary>
        /// creates a new instance of the PropertyMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public PropertyMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        internal new const string Key = "{419bc576-0817-4b7a-8127-f5ad4f7863e7}";
    }
}
