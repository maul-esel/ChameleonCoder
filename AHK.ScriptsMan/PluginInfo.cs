using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHKScriptsMan
{
    public enum PluginType
    {
        undefined,
        lexer
        // add more
    }

    public class PluginInfo
    {
        public string AssemblyName;
        public PluginType PluginType = PluginType.undefined;
    }
}
