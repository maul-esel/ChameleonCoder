namespace ChameleonCoder.Resources.Interfaces
{
    public interface IRichContentResource : IResource
    {
        /// <summary>
        /// validates whether a RichContent member can be applied to this resource or not
        /// </summary>
        /// <param name="member">the RichContentMember to validate</param>
        /// <returns>true if the member can be added, false otherwise</returns>
        bool ValidateRichContent(RichContent.IContentMember member);

        /// <summary>
        /// the collection holding the RichContent
        /// </summary>
        //RichContent.ContentMemberCollection RichContent { get; } //wrong collection
    }
}
