using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// a class managing the plugins installed
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.None), ComSourceInterfaces(typeof(IPluginManagerEvents))]
    public sealed class PluginManager : IPluginManager
    {
        internal PluginManager(IChameleonCoderApp app)
        {
            App = app;
        }

        public IChameleonCoderApp App { get; private set; }

        private bool hasLoadedInstalled = false;

        /// <summary>
        /// loads all assemblies in the \Components\ folder and the contained plugins (if installed)
        /// </summary>
        public void LoadInstalledPlugins()
        {
            if (!hasLoadedInstalled)
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
                            ChameleonCoderApp.Log("ChameleonCoder.Plugins.PluginManager->LoadInstalledPlugins()",
                                "could not load assembly '" + dll + "'",
                                e.ToString());
                        }

                        if (ass != null
                            && ass.IsFullyTrusted
                            && Attribute.IsDefined(ass, typeof(CCPluginAttribute)))
                        {
                            Load(ass.GetTypes());
                        }
                    });
                hasLoadedInstalled = true;
            }
        }

        /// <summary>
        /// loads the given plugins, if installed
        /// </summary>
        /// <param name="plugins">the plugins to load</param>
        internal void Load(IEnumerable<Type> plugins)
        {
            Parallel.ForEach(Filter(plugins), plugin => Add(plugin));
        }

        /// <summary>
        /// filters a list of types representing plugins
        /// </summary>
        /// <param name="types">the list to filter</param>
        /// <returns>the filtered list</returns>
        private IEnumerable<Type> Filter(IEnumerable<Type> types)
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
        private void Add(Type component)
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
                templateDict.TryAdd(template.Identifier, template); // ...store it
                template.Initialize(App); // ... and initialize it
            }

            IService service = plugin as IService;
            if (service != null) // if it is a service:
            {
                serviceDict.TryAdd(service.Identifier, service); // ...store it
                service.Initialize(App); // ... and initialize it
            }

            ILanguageModule module = plugin as ILanguageModule;
            if (module != null) // if it is a Language module
            {
                moduleDict.TryAdd(module.Identifier, module); // ...store it
                module.Initialize(App); // ... and initialize it
            }

            IResourceFactory resourceFactory = plugin as IResourceFactory;
            if (resourceFactory != null) // if it is a ResourceFactory
            {
                resourceFactoryDict.TryAdd(resourceFactory.Identifier, resourceFactory); // ...store it
                resourceFactory.Initialize(App); // ... and initialize it
            }

            IRichContentFactory contentFactory = plugin as IRichContentFactory;
            if (contentFactory != null) // if it is a RichContentFactory
            {
                richContentFactoryDict.TryAdd(contentFactory.Identifier, contentFactory); // ...store it
                contentFactory.Initialize(App); // ... and initialize it
            }
        }

        /// <summary>
        /// calls IPlugin.Shutdown() on all plugins and clears the instance
        /// </summary>
        public void Shutdown()
        {
            UnloadModule();

            foreach (IPlugin plugin in Plugins)
                plugin.Shutdown(); // inform plugins

            // clear lists
            moduleDict.Clear();
            serviceDict.Clear();
            templateDict.Clear();
            resourceFactoryDict.Clear();
            richContentFactoryDict.Clear();

            App = null;
        }

        /// <summary>
        /// returns a list of all registered plugins
        /// </summary>
        /// <returns>the list of plugins</returns>
        public IPlugin[] Plugins
        {
            get
            {
                var plugins = new List<IPlugin>();

                plugins.AddRange(moduleDict.Values);
                plugins.AddRange(serviceDict.Values);
                plugins.AddRange(templateDict.Values);
                plugins.AddRange(resourceFactoryDict.Values);
                plugins.AddRange(richContentFactoryDict.Values);

                return plugins.ToArray();
            }
        }

        public void InstallPermanently(IPlugin plugin)
        {
            Settings.ChameleonCoderSettings.Default.InstalledPlugins.Add(plugin.Identifier.ToString("n"));

            Type pluginType = plugin.GetType();
            string pluginLocation = pluginType.Assembly.Location;

            if (Path.GetDirectoryName(pluginLocation) != Path.Combine(ChameleonCoderApp.AppDir, "Components"))
            {
                File.Copy(pluginLocation,
                    Path.Combine(ChameleonCoderApp.AppDir, "Components\\",
                    Path.GetFileName(pluginLocation)));
            }

            OnPluginInstalled(plugin);

            Load(new Type[1] { pluginType });
        }

        public void UninstallPermanently(IPlugin plugin)
        {
            Settings.ChameleonCoderSettings.Default.InstalledPlugins.Remove(plugin.Identifier.ToString("n"));
            OnPluginUninstalled(plugin);
        }

        #region ILanguageModule

        /// <summary>
        /// a dictionary containing the modules loaded
        /// </summary>
        ConcurrentDictionary<Guid, ILanguageModule> moduleDict = new ConcurrentDictionary<Guid, ILanguageModule>();

        /// <summary>
        /// gets the currently loaded module
        /// </summary>
        public ILanguageModule ActiveModule { get; private set; }

        /// <summary>
        /// returns the count of Language modules registered
        /// </summary>
        public int ModuleCount { get { return moduleDict.Count; } }

        /// <summary>
        /// loads a Language module given its identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <exception cref="ArgumentException">thrown if no module with this identifier is registered.</exception>
        public void LoadModule(ILanguageModule module)
        {
            OnModuleLoad(module);

            module.Load();
            ActiveModule = module;

            /*
             * moved to MainWindow using event handler for IF.ModuleLoaded event
            if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
            {
                ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                    ChameleonCoderApp.Window.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                    module.Name, module.Version, module.Author, module.About)));
            }
            else
                ChameleonCoderApp.Window.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                    module.Name, module.Version, module.Author, module.About);
            */

            OnModuleLoaded(module);
        }

        /// <summary>
        /// unloads the currently loaded module
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if no module is loaded.</exception>
        public void UnloadModule()
        {
            if (ActiveModule == null)
                throw new InvalidOperationException("Module cannot be unloaded: no module loaded!");

            ILanguageModule module = ActiveModule;
            OnModuleUnload(ActiveModule);

            ActiveModule.Unload();
            ActiveModule = null;

            /*
             * moved to Mainwindow using event handler for IF.ModuleUnloaded event
            if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
            {
                ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                    ChameleonCoderApp.Window.CurrentModule.Text = string.Empty));
            }
            else
                ChameleonCoderApp.Window.CurrentModule.Text = string.Empty;
            */

            OnModuleUnloaded(module);
        }

        /// <summary>
        /// returns a module instance given the identifier
        /// </summary>
        /// <param name="id">the module's identifier</param>
        /// <returns>the ILanguageModule instance</returns>
        /// <exception cref="ArgumentException">thrown if no module with this identifier is registered</exception>
        public ILanguageModule GetModule(Guid id)
        {
            ILanguageModule module;
            if (moduleDict.TryGetValue(id, out module))
                return module;
            throw new ArgumentException("this module is not registered!\nGuid: " + id.ToString("b"));
        }

        /// <summary>
        /// checks if a module with a given identifier is registered
        /// </summary>
        /// <param name="id">the module's identifier</param>
        /// <returns>true if the module is registered, false otherwise</returns>
        public bool IsModuleRegistered(Guid id)
        {
            return moduleDict.ContainsKey(id);
        }

        /// <summary>
        /// gets a list with all registered modules
        /// </summary>
        /// <returns>a list with all registered modules</returns>
        public ILanguageModule[] Modules
        {
            get
            {
                return new List<ILanguageModule>(moduleDict.Values).ToArray();
            }
        }

        #endregion

        #region IService

        ConcurrentDictionary<Guid, IService> serviceDict = new ConcurrentDictionary<Guid, IService>();

        /// <summary>
        /// gets the count of registered services
        /// </summary>
        public int ServiceCount { get { return serviceDict.Count; } }

        /// <summary>
        /// calls a service given its identifier
        /// </summary>
        /// <param name="id">the service's identifier</param>
        public void CallService(IService service)
        {
            OnServiceExecute(service);

            /*
             * moved to Mainwindow using event handler for IF.ServiceExecute
            ChameleonCoderApp.Window.CurrentActionProgress.IsIndeterminate = true;
            ChameleonCoderApp.Window.CurrentAction.Text = string.Format(Properties.Resources.ServiceInfo, service.Name, service.Version, service.Author, service.About);
            */

            service.Execute();
            while (service.IsBusy)
                System.Threading.Thread.Sleep(100);

            /*
             * moved to Mainwindow using event handler for IF.ServiceExecuted
            ChameleonCoderApp.Window.CurrentActionProgress.IsIndeterminate = false;
            ChameleonCoderApp.Window.CurrentAction.Text = string.Empty;
            */

            OnServiceExecuted(service);
        }

        /// <summary>
        /// gets a service given its identifier
        /// </summary>
        /// <param name="id">the service's identifier</param>
        /// <returns>the IService instance</returns>
        /// <exception cref="ArgumentException">thrown if no service with this identifier is registered</exception>
        public IService GetService(Guid id)
        {
            IService service;
            if (serviceDict.TryGetValue(id, out service))
                return service;
            throw new ArgumentException("this service is not registered!\nGuid: " + id.ToString("b"));
        }

        /// <summary>
        /// gets a list with all registered services
        /// </summary>
        /// <returns>a list with all registered services</returns>
        public IService[] Services
        {
            get
            {
                return new List<IService>(serviceDict.Values).ToArray();
            }
        }

        public bool IsServiceRegistered(Guid id)
        {
            return serviceDict.ContainsKey(id);
        }

        #endregion

        #region ITemplate

        ConcurrentDictionary<Guid, ITemplate> templateDict = new ConcurrentDictionary<Guid, ITemplate>();

        /// <summary>
        /// gets the count of registered templates
        /// </summary>
        public int TemplateCount { get { return templateDict.Count; } }

        /// <summary>
        /// gets a list with all registered templates
        /// </summary>
        /// <returns>a list with all registered templates</returns>
        public ITemplate[] Templates
        {
            get
            {
                return new List<ITemplate>(templateDict.Values).ToArray();
            }
        }

        public ITemplate GetTemplate(Guid id)
        {
            ITemplate template;
            if (templateDict.TryGetValue(id, out template))
                return template;
            throw new ArgumentException("this template is not registered!\nGuid: " + id.ToString("b"));
        }

        public bool IsTemplateRegistered(Guid id)
        {
            return templateDict.ContainsKey(id);
        }

        #endregion

        #region IResourceFactory

        ConcurrentDictionary<Guid, IResourceFactory> resourceFactoryDict = new ConcurrentDictionary<Guid, IResourceFactory>();

        /// <summary>
        /// gets the count of registered IResourceFactory
        /// </summary>
        public int ResourceFactoryCount { get { return resourceFactoryDict.Count; } }

        /// <summary>
        /// gets a list of all registered IResourceFactory
        /// </summary>
        /// <returns>the list</returns>
        public IResourceFactory[] ResourceFactories
        {
            get
            {
                return new List<IResourceFactory>(resourceFactoryDict.Values).ToArray();
            }
        }

        public bool IsResourceFactoryRegistered(IResourceFactory factory)
        {
            return resourceFactoryDict.Values.Contains(factory);
        }

        public IResourceFactory GetResourceFactory(Guid id)
        {
            IResourceFactory resFactory;
            if (resourceFactoryDict.TryGetValue(id, out resFactory))
                return resFactory;
            throw new ArgumentException("this resource factory is not registered!\nGuid: " + id.ToString("b"));
        }

        public bool IsResourceFactoryRegistered(Guid id)
        {
            return resourceFactoryDict.ContainsKey(id);
        }

        #endregion

        #region IRichContentFactory

        ConcurrentDictionary<Guid, IRichContentFactory> richContentFactoryDict = new ConcurrentDictionary<Guid, IRichContentFactory>();

        /// <summary>
        /// gets the count of registered IRichContentFactory
        /// </summary>
        public int RichContentFactoryCount { get { return richContentFactoryDict.Count; } }

        /// <summary>
        /// gets a list of all registered IRichContentFactory
        /// </summary>
        /// <returns>the list</returns>
        public IRichContentFactory[] RichContentFactories
        {
            get
            {
                return new List<IRichContentFactory>(richContentFactoryDict.Values).ToArray();
            }
        }

        public bool IsRichContentFactoryRegistered(IRichContentFactory factory)
        {
            return richContentFactoryDict.Values.Contains(factory);
        }

        public IRichContentFactory GetRichContentFactory(Guid id)
        {
            IRichContentFactory rcFactory;
            if (richContentFactoryDict.TryGetValue(id, out rcFactory))
                return rcFactory;
            throw new ArgumentException("this RichContent factory is not registered!\nGuid: " + id.ToString("b"));
        }

        public bool IsRichContentFactoryRegistered(Guid id)
        {
            return richContentFactoryDict.ContainsKey(id);
        }

        #endregion

        #region events

        /// <summary>
        /// raised when a Language module is going to be loaded
        /// </summary>
        public event LanguageModuleEventHandler ModuleLoad;

        /// <summary>
        /// raised when a Language module was loaded
        /// </summary>
        public event LanguageModuleEventHandler ModuleLoaded;

        /// <summary>
        /// raised when a Language module is going to be unloaded
        /// </summary>
        public event LanguageModuleEventHandler ModuleUnload;

        /// <summary>
        /// raised when a Language module was unloaded
        /// </summary>
        public event LanguageModuleEventHandler ModuleUnloaded;

        /// <summary>
        /// raised when a service is going to be executed
        /// </summary>
        public event ServiceEventHandler ServiceExecute;

        /// <summary>
        /// raised when a service was executed
        /// </summary>
        public event ServiceEventHandler ServiceExecuted;

        public event PluginEventHandler PluginInstalled;

        public event PluginEventHandler PluginUninstalled;

        #endregion

        #region event infrastructure

        /// <summary>
        /// raises the ModuleLoad event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        private void OnModuleLoad(ILanguageModule module)
        {
            LanguageModuleEventHandler handler = ModuleLoad;
            if (handler != null)
                handler(this, new ModuleEventArgs(module));
        }

        /// <summary>
        /// raises the ModuleLoaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        private void OnModuleLoaded(ILanguageModule module)
        {
            LanguageModuleEventHandler handler = ModuleLoaded;
            if (handler != null)
                handler(this, new ModuleEventArgs(module));
        }

        /// <summary>
        /// raises the ModuleUnload event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        private void OnModuleUnload(ILanguageModule module)
        {
            LanguageModuleEventHandler handler = ModuleUnload;
            if (handler != null)
                handler(this, new ModuleEventArgs(module));
        }

        /// <summary>
        /// raises the ModuleUnloaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        private void OnModuleUnloaded(ILanguageModule module)
        {
            LanguageModuleEventHandler handler = ModuleUnloaded;
            if (handler != null)
                handler(this, new ModuleEventArgs(module));
        }

        /// <summary>
        /// raises the ServiceExecute event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        private void OnServiceExecute(IService service)
        {
            ServiceEventHandler handler = ServiceExecute;
            if (handler != null)
                handler(this, new ServiceEventArgs(service));
        }

        /// <summary>
        /// raises the ServiceExecuted event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        private void OnServiceExecuted(IService service)
        {
            ServiceEventHandler handler = ServiceExecuted;
            if (handler != null)
                handler(this, new ServiceEventArgs(service));
        }

        private void OnPluginInstalled(IPlugin plugin)
        {
            PluginEventHandler handler = PluginInstalled;
            if (handler != null)
                handler(this, new PluginEventArgs(plugin));
        }

        private void OnPluginUninstalled(IPlugin plugin)
        {
            PluginEventHandler handler = PluginUninstalled;
            if (handler != null)
                handler(this, new PluginEventArgs(plugin));
        }

        #endregion
    }
}
