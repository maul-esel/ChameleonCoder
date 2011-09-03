namespace ChameleonCoder.Resources.RichContent
{
    /// <summary>
    /// an interface describing a member of the RichContent some resources can have.
    /// </summary>
    public interface IContentMember
    {
        /// <summary>
        /// the list of childMembers the content member has
        /// </summary>
        RichContentCollection childMembers { get; }

        /// <summary>
        /// gets the member's html representation
        /// </summary>
        /// <param name="param">optional information for the member</param>
        /// <returns>the member's representation</returns>
        /// <remarks>the rendering of an member is decided by its parent member.
        /// For example, the parent member can integrate the html given by GetHtml(),
        /// but it can also just use a property of the child member and add it,
        /// or it can even ignore it.</remarks>
        string GetHtml(object param = null);

        /// <summary>
        /// saves the instance
        /// </summary>
        void Save();

        /// <summary>
        /// initializes the instance
        /// </summary>
        /// <param name="node">the XmlElement containing the member's data</param>
        void Init(System.Xml.XmlElement node);
    }
}
