using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    [ComVisible(true), Guid("E1FBC046-F102-4B60-A4C7-36257737399F"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IResourceEventArgs
    {
        IResource Resource { get; }
    }
}
