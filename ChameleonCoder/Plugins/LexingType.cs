using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an enume defining the type of lexing performed by a language module
    /// </summary>
    public enum LexingType
    {
        /// <summary>
        /// no lexing performed
        /// </summary>
        none,

        /// <summary>
        /// a custom DocumentHighlighter was written
        /// </summary>
        custom,

        /// <summary>
        /// the XML-highlighting engine should be used
        /// </summary>
        xml,

        /// <summary>
        /// an AvalonEdit-integrated engine should be used
        /// </summary>
        integrated
    }
}
