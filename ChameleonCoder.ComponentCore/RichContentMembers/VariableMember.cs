using System;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a variable
    /// </summary>
    public class VariableMember : IContentMember
    {
        public VariableMember(System.Xml.XmlElement node, IContentMember parent)
        {
        }

        /// <summary>
        /// gets the collection of child members
        /// </summary>
        public RichContentCollection Children { get { return childrenCollection; } }

        private readonly RichContentCollection childrenCollection = new RichContentCollection();

        /// <summary>
        /// gets the member's HTML representation
        /// </summary>
        /// <param name="param">a parameter passed to the method, no special use</param>
        /// <returns>the representation as HTML text</returns>
        public string GetHtml(object param)
        {
            return string.Empty;
        }

        /// <summary>
        /// saves the current instance
        /// </summary>
        public virtual void Save() { }

        /// <summary>
        /// gets the member's parent member
        /// </summary>
        public IContentMember Parent { get; protected set; }

        /// <summary>
        /// gets the name of the member
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// gets the identifier of the member
        /// </summary>
        public Guid Identifier { get; protected set; }

        /// <summary>
        /// gets the icon representing this instance to the user
        /// </summary>
        /// <value>null, as this is not yet implemented</value>
        public System.Windows.Media.ImageSource Icon { get; private set; }

        internal const string Key = "{23f3716c-08a1-44d3-8a54-ac01c29435a2}";
    }
}
