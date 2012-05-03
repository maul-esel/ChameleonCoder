using System.Runtime.InteropServices;

namespace ChameleonCoder
{
    [ComVisible(true), Guid("B30D84E7-5976-4C87-BCFD-E917F0E29712"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IAppComponent
    {
        IChameleonCoderApp App { get; }
    }
}
