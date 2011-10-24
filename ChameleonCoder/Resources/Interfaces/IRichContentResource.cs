namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to implement by resources that can host RichContent
    /// </summary>
    public interface IRichContentResource : IResource
    {
        /// <summary>
        /// returns the Html representing the resource's RichContent
        /// </summary>
        /// <returns>the Html text</returns>
        string GetHtml();

        /// <summary>
        /// registers a style rule
        /// </summary>
        /// <param name="classStyle">the style to register</param>
        /// <remarks>Implementors can decide whether they really implement this or provide own CSS.
        /// In case this is not 'really' implemented, just do nothing.</remarks>
        void RegisterClassStyle(CssClassStyle classStyle);

        /// <summary>
        /// the collection holding the RichContent
        /// </summary>
        RichContent.RichContentCollection RichContent { get; }
    }
}
