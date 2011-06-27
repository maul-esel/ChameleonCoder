using System;

namespace ChameleonCoder.Services
{
    public interface IServiceHost : IPluginHost
    {
        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
