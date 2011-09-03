namespace ChameleonCoder.Resources.Interfaces
{
    public interface IRichContentResource : IResource
    {
        /// <summary>
        /// returns the Html representing the resource's RichContent
        /// </summary>
        /// <returns>the Html text</returns>
        string GetHtml();

        /// <summary>
        /// the collection holding the RichContent
        /// </summary>
        RichContent.RichContentCollection RichContent { get; }
    }
}
