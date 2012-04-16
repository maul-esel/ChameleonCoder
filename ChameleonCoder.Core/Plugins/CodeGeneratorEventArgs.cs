using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// Custom EventArgs class for CodeGenerators
    /// </summary>
    public sealed class CodeGeneratorEventArgs : EventArgs
    {
        /// <summary>
        /// defines whether code insertion was already handled, e.g. through an extra resource
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// the generated code
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// a delegate for CodeGenerators
    /// </summary>
    /// <param name="sender">the resource to be worked on</param>
    /// <param name="args">additional data</param>
    public delegate void CodeGeneratorEventHandler(Resources.Interfaces.IResource sender, CodeGeneratorEventArgs args);
}
