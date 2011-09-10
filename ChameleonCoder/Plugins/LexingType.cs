namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an enum defining the type of lexing performed by a Language module
    /// </summary>
    public enum LexingType
    {
        /// <summary>
        /// no lexing performed
        /// </summary>
        None,

        /// <summary>
        /// a Custom DocumentHighlighter was written
        /// </summary>
        Custom,

        /// <summary>
        /// the XML-highlighting engine should be used
        /// </summary>
        Xml,

        /// <summary>
        /// an AvalonEdit-Integrated engine should be used
        /// </summary>
        Integrated
    }
}
