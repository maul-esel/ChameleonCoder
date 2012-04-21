using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// the interface for a service
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("46ab262d-3696-4a82-a7a6-ab36e749b7fd")]
    public interface IService : IPlugin
    {
        /// <summary>
        /// called when the user starts the service
        /// </summary>
        void Execute();

        /// <summary>
        /// defines whether the service is busy or not
        /// </summary>
        bool IsBusy { get; }
    }
}
