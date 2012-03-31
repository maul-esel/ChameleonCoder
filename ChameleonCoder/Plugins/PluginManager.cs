using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Threading.Tasks;
using IF = ChameleonCoder.Shared.InformationProvider;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// a static class managing the plugins installed
    /// </summary>
    public static class PluginManager
    {
        /// <summary>
        /// loads all assemblies in the \Components\ folder and the contained plugins (if installed)
        /// </summary>
        internal static void Load()
        {
            Parallel.ForEach(Directory.GetFiles(Path.Combine(ChameleonCoderApp.AppDir, "Components"), "*.dll"),
                dll =>
                {
                    System.Reflection.Assembly ass = null;
                    try
                    {
                        ass = System.Reflection.Assembly.LoadFile(dll);
                    }
                    catch (BadImageFormatException e)
                    {
                        ChameleonCoderApp.Log("ChameleonCoder.Plugins.PluginManager->Load()",
                            "could not assembly '" + dll + "'",
                            e.ToString());
                    }

                    if (ass != null
                        && ass.IsFullyTrusted
                        && Attribute.IsDefined(ass, typeof(CCPluginAttribute)))
                    {
                        Load(ass.GetTypes());
                    }
                });
        }

        /// <summary>
        /// loads the given plugins, if installed
        /// </summary>
        /// <param name="plugins">the plugins to load</param>
        internal static void Load(IEnumerable<Type> plugins)
        {
            Parallel.ForEach(Filter(plugins), plugin => Add(plugin));
        }

        /// <summary>
        /// filters a list of types representing plugins
        /// </summary>
        /// <param name="types">the list to filter</param>
        /// <returns>the filtered list</returns>
        private static IEnumerable<Type> Filter(IEnumerable<Type> types)
        {
            ConcurrentBag<Type> filtered = new ConcurrentBag<Type>();

            Parallel.ForEach(types, type =>
                {
                    if (Attribute.IsDefined(type, typeof(CCPluginAttribute)) // filter non-plugin types
                        && !type.IsAbstract && type.IsClass && type.IsPublic // filter types that can't be instantiated
                        && type.GetInterface(typeof(IPlugin).FullName) != null // only use those that implement plugin interface
                        && type.GetConstructor(Type.EmptyTypes) != null) // filter types without parameterless constructor
                        filtered.Add(type); // if everything ok: add it to the list
                });

            return filtered; // return the list
        }

        /// <summary>
        /// adds a type to the corresponding plugin collections
        /// </summary>
        /// <param name="component">the type to add</param>
        private static void Add(Type component)
        {
            IPlugin plugin = Activator.CreateInstance(component) as IPlugin; // create an instance
            if (plugin == null) // check for null
                return;

            // if this plugin is not registered
            if (!Settings.ChameleonCoderSettings.Default.InstalledPlugins.Contains(plugin.Identifier.ToString("n")))
                return;

            ITemplate template = plugin as ITemplate;
            if (template != null) // if it is a template:
            {
                Templates.TryAdd(template.Identifier, template); // ...store it
                template.Initialize(); // ... and initialize it
            }

            IService service = plugin as IService;
            if (service != null) // if it is a service:
            {
                Services.TryAdd(service.Identifier, service); // ...store it
                service.Initialize(); // ... and initialize it
            }

            ILanguageModule module = plugin as ILanguageModule;
            if (module != null) // if it is a Language module
            {
                Modules.TryAdd(module.Identifier, module); // ...store it
                module.Initialize(); // ... and initialize it
            }

            IResourceFactory resourceFactory = plugin as IResourceFactory;
            if (resourceFactory != null) // if it is a ResourceFactory
            {
                ResourceFactories.TryAdd(resourceFactory.Identifier, resourceFactory); // ...store it
                resourceFactory.Initialize(); // ... and initialize it
            }

            IRichContentFactory contentFactory = plugin as IRichContentFactory;
            if (contentFactory != null) // if it is a RichContentFactory
            {
                RichContentFactories.TryAdd(contentFactory.Identifier, contentFactory); // ...store it
                contentFactory.Initialize(); // ... and initialize it
            }
        }

        /// <summary>
        /// calls IPlugin.Shutdown() on all plugins
        /// </summary>
        internal static void Shutdown()
        {
            foreach (IPlugin plugin in GetPlugins())
                plugin.Shutdown();
        }

        /// <summary>
        /// returns a list of all registered plugins
        /// </summary>
        /// <returns>the list of plugins</returns>
        internal static IEnumerable<IPlugin> GetPlugins()
        {
            var plugins = new List<IPlugin>();

            plugins.AddRange(GetModules());
            plugins.AddRange(GetServices());
            plugins.AddRange(GetTemplates());
            plugins.AddRange(GetResourceFactories());
            plugins.AddRange(GetRichContentFactories());

            return plugins;
        }

        #region ILanguageModule

        /// <summary>
        /// a dictionary containing the modules loaded
        /// </summary>
        static ConcurrentDictionary<Guid, ILanguageModule> Modules = new ConcurrentDictionary<Guid, ILanguageModule>();

        /// <summary>
        /// gets the currently loaded module
        /// </summary>
        internal static ILanguageModule ActiveModule { get; private set; }

        /// <summary>
        /// returns the count of Language modules registered
        /// </summary>
        internal static int ModuleCount { get { return Modules.Count; } }

        /// <summary>
        /// loads a Language module given its identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <exception cref="ArgumentException">thrown if no module with this identifier is registered.</exception>
        internal static void LoadModule(Guid id)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(id, out module))
            {
                IF.OnModuleLoad(module, new EventArgs());

                module.Load();
                ActiveModule = module;

                if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
                {
                    ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                        ChameleonCoderApp.Gui.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        module.Name, module.Version, module.Author, module.About)));
                }
                else
                    ChameleonCoderApp.Gui.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        module.Name, module.Version, module.Author, module.About);

                IF.OnModuleLoaded(module, new EventArgs());
            }
            else
                throw new ArgumentException("this module is not registered!\nGuid: " + id.ToString("b"));
        }

        /// <summary>
        /// unloads the currently loaded module
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if no module is loaded.</exception>
        internal static void UnloadModule()
        {
            if (ActiveModule == null)
                throw new InvalidOperationException("Module cannot be unloaded: no module loaded!");

            ILanguageModule module = ActiveModule;
            IF.OnModuleUnload(ActiveModule, new EventArgs());

            ActiveModule.Unload();
            ActiveModule = null;

            if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
            {
                ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                    ChameleonCoderApp.Gui.CurrentModule.Text = string.Empty));
            }
            else
                ChameleonCoderApp.Gui.CurrentModule.Text = string.Empty;

            IF.OnModuleUnloaded(module, new EventArgs());
        }

        /// <summary>
        /// returns a module instance given the identifier
        /// </summary>
        /// <param name="id">the module's identifier</param>
        /// <returns>the ILanguageModule instance</returns>
        /// <exception cref="ArgumentException">thrown if no module with this identifier is registered</exception>
        public static ILanguageModule GetModule(Guid id)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(id, out module))
                return module;
            throw new ArgumentException("this module is not registered!\nGuid: " + id.ToString("b"));
        }

        /// <summary>
        /// tries to get a module instance given its identifier
        /// </summary>
        /// <param name="id">the module's identifier</param>
        /// <param name="module">the ILanguageModule instance</param>
        /// <returns>true on success, false otherwise</returns>
        public static bool TryGetModule(Guid id, out ILanguageModule module)
        {
            return Modules.TryGetValue(id, out module);
        }

        /// <summary>
        /// checks if a module with a given identifier is registered
        /// </summary>
        /// <param name="id">the module's identifier</param>
        /// <returns>true if the module is registered, false otherwise</returns>
        public static bool IsModuleRegistered(Guid id)
        {
            return Modules.ContainsKey(id);
        }

        /// <summary>
        /// gets a list with all registered modules
        /// </summary>
        /// <returns>a list with all registered modules</returns>
        public static IEnumerable<ILanguageModule> GetModules()
        {
            return Modules.Values;
        }

        #endregion

        #region IService

        static ConcurrentDictionary<Guid, IService> Services = new ConcurrentDictionary<Guid, IService>();

        /// <summary>
        /// gets the count of registered services
        /// </summary>
        internal static int ServiceCount { get { return Services.Count; } }

        /// <summary>
        /// calls a service given its identifier
        /// </summary>
        /// <param name="id">the service's identifier</param>
        internal static void CallService(Guid id)
        {
            IService service = GetService(id);

            IF.OnServiceExecute(service, new EventArgs());

            ChameleonCoderApp.Gui.CurrentActionProgress.IsIndeterminate = true;
            ChameleonCoderApp.Gui.CurrentAction.Text = string.Format(Properties.Resources.ServiceInfo, service.Name, service.Version, service.Author, service.About);

            service.Execute();
            while (service.IsBusy)
                System.Threading.Thread.Sleep(100);

            ChameleonCoderApp.Gui.CurrentActionProgress.IsIndeterminate = false;
            ChameleonCoderApp.Gui.CurrentAction.Text = string.Empty;

            IF.OnServiceExecuted(service, new EventArgs());
        }

        /// <summary>
        /// gets a service given its identifier
        /// </summary>
        /// <param name="id">the service's identifier</param>
        /// <returns>the IService instance</returns>
        /// <exception cref="ArgumentException">thrown if no service with this identifier is registered</exception>
        internal static IService GetService(Guid id)
        {
            IService service;
            if (Services.TryGetValue(id, out service))
                return service;
            throw new ArgumentException("this service is not registered!\nGuid: " + id.ToString("b"));
        }

        /// <summary>
        /// gets a list with all registered services
        /// </summary>
        /// <returns>a list with all registered services</returns>
        internal static IEnumerable<IService> GetServices()
        {
            return Services.Values;
        }

        #endregion

        #region ITemplate

        static ConcurrentDictionary<Guid, ITemplate> Templates = new ConcurrentDictionary<Guid, ITemplate>();

        /// <summary>
        /// gets the count of registered templates
        /// </summary>
        static int TemplateCount { get { return Templates.Count; } }

        /// <summary>
        /// gets a list with all registered templates
        /// </summary>
        /// <returns>a list with all registered templates</returns>
        internal static IEnumerable<ITemplate> GetTemplates()
        {
            return Templates.Values;
        }

        #endregion

        #region IResourceFactory

        static ConcurrentDictionary<Guid, IResourceFactory> ResourceFactories = new ConcurrentDictionary<Guid, IResourceFactory>();

        /// <summary>
        /// gets the count of registered IResourceFactory
        /// </summary>
        internal static int ResourceFactoryCount { get { return ResourceFactories.Count; } }

        /// <summary>
        /// gets a list of all registered IResourceFactory
        /// </summary>
        /// <returns>the list</returns>
        internal static IEnumerable<IResourceFactory> GetResourceFactories()
        {
            return ResourceFactories.Values;
        }

        internal static bool IsResourceFactoryRegistered(IResourceFactory factory)
        {
            return ResourceFactories.Values.Contains(factory);
        }

        #endregion

        #region IRichContentFactory

        static ConcurrentDictionary<Guid, IRichContentFactory> RichContentFactories = new ConcurrentDictionary<Guid, IRichContentFactory>();

        /// <summary>
        /// gets the count of registered IRichContentFactory
        /// </summary>
        internal static int RichContentFactoryCount { get { return RichContentFactories.Count; } }

        /// <summary>
        /// gets a list of all registered IRichContentFactory
        /// </summary>
        /// <returns>the list</returns>
        internal static IEnumerable<IRichContentFactory> GetRichContentFactories()
        {
            return RichContentFactories.Values;
        }

        internal static bool IsRichContentFactoryRegistered(IRichContentFactory factory)
        {
            return RichContentFactories.Values.Contains(factory);
        }

        #endregion
    }
}
