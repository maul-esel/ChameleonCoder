namespace ChameleonCoder.Resources.RichContent
{
    /// <summary>
    /// an interface describing a member of the RichContent some resources can have.
    /// </summary>
    public interface IContentMember : IComponent
    {
        /// <summary>
        /// the user's summary of the member
        /// </summary>
        string Summary { get; }

        /// <summary>
        /// the list of Children the content member has
        /// </summary>
        RichContentCollection Children { get; }

        /// <summary>
        /// the member's parent member
        /// </summary>
        IContentMember Parent { get; }

        /// <summary>
        /// the resource the member belongs to
        /// </summary>
        Interfaces.IRichContentResource Resource { get; }

        /// <summary>
        /// a list of related members' ids
        /// </summary>
        System.Collections.Generic.IEnumerable<System.Guid> Related { get; }

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
    }
}
