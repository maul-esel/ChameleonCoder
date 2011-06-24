using System;

namespace ChameleonCoder.Plugins
{
    public interface IServiceHost : IPluginHost
    {
        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
