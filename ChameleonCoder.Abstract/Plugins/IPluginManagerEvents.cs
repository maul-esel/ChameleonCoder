using System;
using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    #region event delegate types

    /// <summary>
    /// a delegate for LanguageModule events
    /// </summary>
    /// <param name="sender">the LanguageModule raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void LanguageModuleEventHandler(object sender, IModuleEventArgs e);

    /// <summary>
    /// a delegate for Service Events
    /// </summary>
    /// <param name="sender">the service raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void ServiceEventHandler(object sender, IServiceEventArgs e);

    public delegate void PluginEventHandler(object sender, IPluginEventArgs e);

    #endregion

    [ComVisible(true), Guid("DDD26D2B-B1AF-49E9-A90C-D45DB9ADC70A"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPluginManagerEvents
    {
        void ModuleLoad(object sender, IModuleEventArgs e);
        void ModuleLoaded(object sender, IModuleEventArgs e);

        void ModuleUnload(object sender, IModuleEventArgs e);
        void ModuleUnloaded(object sender, IModuleEventArgs e);

        void ServiceExecute(object sender, IServiceEventArgs e);
        void ServiceExecuted(object sender, IServiceEventArgs e);

        void PluginInstalled(object sender, IPluginEventArgs e);
        void PluginUninstalled(object sender, IPluginEventArgs e);
    }
}
