using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.LanguageModules
{
    public class CodeGeneratorEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public string Code { get; set; }
    }
}
