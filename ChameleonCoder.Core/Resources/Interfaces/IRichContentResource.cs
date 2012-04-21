using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to implement by resources that can host RichContent
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("ea878a6b-6e41-42b5-9c3e-056982eeb796")]
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
