using System;

namespace ChameleonCoder.Plugins.Services
{
    public interface IServiceHost : IPluginHost
    {
        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
