using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), Guid("7FEBD063-04F8-4973-ADC5-430A9287EE6F"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPluginEventArgs
    {
        IPlugin Plugin { get; }
    }
}


