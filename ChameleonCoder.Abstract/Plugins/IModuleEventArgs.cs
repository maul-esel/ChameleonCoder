using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), Guid("4557D702-FC12-4F8A-A995-EC44889139E4"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IModuleEventArgs
    {
        ILanguageModule Module { get; }
    }
}
