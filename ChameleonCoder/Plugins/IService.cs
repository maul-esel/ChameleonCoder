using System;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// the interface for a service
    /// </summary>
    public interface IService : IPlugin
    {
        /// <summary>
        /// called when the user starts the service
        /// </summary>
        void Call();

        /// <summary>
        /// defines whether the service is busy or not
        /// </summary>
        bool IsBusy { get; }
    }
}
