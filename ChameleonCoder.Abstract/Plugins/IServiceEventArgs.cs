using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), Guid("D88AB545-DE78-4996-A7A0-3A2411FE05B4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IServiceEventArgs
    {
        IService Service { get; }
    }
}

