using System;
using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// Custom EventArgs class for CodeGenerators
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
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
    [ComVisible(true)]
    public delegate void CodeGeneratorEventHandler(Resources.IResource sender, CodeGeneratorEventArgs args);
}
