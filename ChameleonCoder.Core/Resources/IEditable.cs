using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// an interface to implement by resources that can be edited
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("e02b3d17-d888-4138-8fc7-41b24024bfb4")]
    public interface IEditable : IResource
    {
        /// <summary>
        /// gets the text that will be edited
        /// </summary>
        /// <returns>the text to edit</returns>
        string GetText();

        /// <summary>
        /// saves the edited text
        /// </summary>
        /// <param name="text">the modified text</param>
        void SaveText(string text);
    }
}
