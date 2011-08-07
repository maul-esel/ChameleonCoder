using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Plugins
{
    public class CodeGeneratorEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public string Code { get; set; }
    }

    public delegate void CodeGeneratorEventHandler(Resources.Interfaces.IResource sender, CodeGeneratorEventArgs e);
}
