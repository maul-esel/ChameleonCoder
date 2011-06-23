using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Plugins
{
    public interface IServiceHost : IPluginHost
    {
        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
