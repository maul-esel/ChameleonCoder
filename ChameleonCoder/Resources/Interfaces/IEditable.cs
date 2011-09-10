namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to implement by resources that can be edited
    /// </summary>
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
