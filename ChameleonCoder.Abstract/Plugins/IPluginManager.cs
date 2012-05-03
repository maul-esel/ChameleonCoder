using System;
using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), Guid("9AEC72FA-92F8-4521-B30A-D99F7F3B0019"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPluginManager : IAppComponent
    {
        void LoadInstalledPlugins();
        void Shutdown();

        void InstallPermanently(IPlugin plugin);
        void UninstallPermanently(IPlugin plugin);

        IPlugin[] Plugins { get; }

        #region modules

        int ModuleCount { get; }
        ILanguageModule[] Modules { get; }

        ILanguageModule ActiveModule { get; }
        void LoadModule(ILanguageModule module);
        void UnloadModule();

        ILanguageModule GetModule(Guid id);
        bool IsModuleRegistered(Guid id);

        #endregion

        #region services

        int ServiceCount { get; }
        IService[] Services { get; }

        void CallService(IService service);

        IService GetService(Guid id);
        bool IsServiceRegistered(Guid id);

        #endregion

        #region templates

        int TemplateCount { get; }
        ITemplate[] Templates { get; }

        ITemplate GetTemplate(Guid id);
        bool IsTemplateRegistered(Guid id);

        #endregion

        #region resource factories

        int ResourceFactoryCount { get; }
        IResourceFactory[] ResourceFactories { get; }

        IResourceFactory GetResourceFactory(Guid id);
        bool IsResourceFactoryRegistered(Guid id);

        #endregion

        #region RichContent factories

        int RichContentFactoryCount { get; }
        IRichContentFactory[] RichContentFactories { get; }

        IRichContentFactory GetRichContentFactory(Guid id);
        bool IsRichContentFactoryRegistered(Guid id);

        #endregion

        #region events

        /*
         * This region includes all the PluginManager events.
         * They are not ComVisible, as COM clients consume them via the IPluginManagerEvents interface.
         * Yet they exist here to be consumed by other parts of the project and .NET plugins.
        */

        /// <summary>
        /// raised when a Language module is going to be loaded
        /// </summary>
        event LanguageModuleEventHandler ModuleLoad;

        /// <summary>
        /// raised when a Language module was loaded
        /// </summary>
        event LanguageModuleEventHandler ModuleLoaded;

        /// <summary>
        /// raised when a Language module is going to be unloaded
        /// </summary>
        event LanguageModuleEventHandler ModuleUnload;

        /// <summary>
        /// raised when a Language module was unloaded
        /// </summary>
        event LanguageModuleEventHandler ModuleUnloaded;

        /// <summary>
        /// raised when a service is going to be executed
        /// </summary>
        event ServiceEventHandler ServiceExecute;

        /// <summary>
        /// raised when a service was executed
        /// </summary>
        event ServiceEventHandler ServiceExecuted;

        event PluginEventHandler PluginInstalled;

        event PluginEventHandler PluginUninstalled;

        #endregion
    }
}
