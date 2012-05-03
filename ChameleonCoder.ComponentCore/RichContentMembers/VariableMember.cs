using System;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a variable
    /// </summary>
    public class VariableMember : IContentMember
    {
        /// <summary>
        /// gets the collection of child members
        /// </summary>
        public IContentMember[] Children { get { return childrenCollection.Values; } }

        private readonly RichContentCollection childrenCollection = new RichContentCollection();

        public void AddChildMember(IContentMember child)
        {
            childrenCollection.Add(child);
        }

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

        public ChameleonCoder.Resources.IRichContentResource Resource { get; private set; }
        public Files.IDataFile File { get; private set; }

        /// <summary>
        /// initializes the instance
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent member</param>
        public virtual void Initialize(System.Xml.XmlElement node, IContentMember parent) { }
    }
}
