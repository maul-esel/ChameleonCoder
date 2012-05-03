using System.Runtime.InteropServices;
using System.Collections.Specialized;

namespace ChameleonCoder.Resources.RichContent
{
    /// <summary>
    /// an interface describing a member of the RichContent some resources can have.
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("e86c2761-74dd-4c32-b2f3-8ff95bcdd03c")]
    public interface IContentMember : IComponent
    {
        /// <summary>
        /// the list of Children the content member has
        /// </summary>
        [System.Obsolete]
        IContentMember[] Children { get; }
        void AddChildMember(IContentMember member);

        /// <summary>
        /// the member's parent member
        /// </summary>
        IContentMember Parent { get; }

        Files.IDataFile File { get; }

        IRichContentResource Resource { get; }

        IObservableStringDictionary Attributes { get; }

        /// <summary>
        /// gets the member's html representation
        /// </summary>
        /// <param name="param">optional information for the member</param>
        /// <returns>the member's representation</returns>
        /// <remarks>the rendering of an member is decided by its parent member.
        /// For example, the parent member can integrate the html given by GetHtml(),
        /// but it can also just use a property of the child member and add it,
        /// or it can even ignore it.</remarks>
        string GetHtml(object param);

        /// <summary>
        /// saves the instance
        /// </summary>
        void Save();

        /// <summary>
        /// initializes the instance with the given information
        /// </summary>
        /// <param name="node">the XmlElement containing the member's data</param>
        /// <param name="parent">the member's parent member, or null if this is a top-level member</param>
        [System.Obsolete("use factory for creation and initialization", true)]
        void Initialize(IObservableStringDictionary data, IContentMember parent, IRichContentResource resource, Files.IDataFile file);
    }
}
