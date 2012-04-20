using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using IF = ChameleonCoder.Shared.InformationProvider;

namespace ChameleonCoder.Plugins
{
    #region event delegate types

    /// <summary>
    /// a delegate for LanguageModule events
    /// </summary>
    /// <param name="sender">the LanguageModule raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void LanguageModuleEventHandler(object sender, EventArgs e);

    /// <summary>
    /// a delegate for Service Events
    /// </summary>
    /// <param name="sender">the service raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void ServiceEventHandler(object sender, EventArgs e);

    #endregion

    /// <summary>
    /// a class managing the plugins installed
    /// </summary>
    [ComVisible(true)]
    public sealed class PluginManager
    {
        internal PluginManager(ChameleonCoderApp app)
        {
            App = app;
        }

        public ChameleonCoderApp App { get; private set; }

        public bool HasLoaded { get; private set; }

        /// <summary>
        /// loads all assemblies in the \Components\ folder and the contained plugins (if installed)
        /// </summary>
        public void Load()
        {
            if (HasLoaded)
                throw new InvalidOperationException("Manager has already been loaded.");

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
            HasLoaded = true;
        }

        /// <summary>
        /// loads the given plugins, if installed
        /// </summary>
        /// <param name="plugins">the plugins to load</param>
        [ComVisible(false)]
        internal void Load(IEnumerable<Type> plugins)
        {
            Parallel.ForEach(Filter(plugins), plugin => Add(plugin));
        }

        /// <summary>
        /// filters a list of types representing plugins
        /// </summary>
        /// <param name="types">the list to filter</param>
        /// <returns>the filtered list</returns>
        [ComVisible(false)]
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
        /// calls IPlugin.Shutdown() on all plugins and clears the instance
        /// </summary>
        public void Shutdown()
        {
            UnloadModule();

            foreach (IPlugin plugin in GetPlugins())
                plugin.Shutdown(); // inform plugins

            // clear lists
            Modules.Clear();
            Services.Clear();
            Templates.Clear();
            ResourceFactories.Clear();
            RichContentFactories.Clear();

            App = null;
        }

        /// <summary>
        /// returns a list of all registered plugins
        /// </summary>
        /// <returns>the list of plugins</returns>
        internal IEnumerable<IPlugin> GetPlugins()
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
        [ComVisible(false)]
        ConcurrentDictionary<Guid, ILanguageModule> Modules = new ConcurrentDictionary<Guid, ILanguageModule>();

        /// <summary>
        /// gets the currently loaded module
        /// </summary>
        public ILanguageModule ActiveModule { get; private set; }

        /// <summary>
        /// returns the count of Language modules registered
        /// </summary>
        public int ModuleCount { get { return Modules.Count; } }

        /// <summary>
        /// loads a Language module given its identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <exception cref="ArgumentException">thrown if no module with this identifier is registered.</exception>
        public void LoadModule(Guid id)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(id, out module))
            {
                OnModuleLoad(module, new EventArgs());

                module.Load();
                ActiveModule = module;

                /*
                 * moved to MainWindow using event handler for IF.ModuleLoaded event
                if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
                {
                    ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                        ChameleonCoderApp.Gui.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        module.Name, module.Version, module.Author, module.About)));
                }
                else
                    ChameleonCoderApp.Gui.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        module.Name, module.Version, module.Author, module.About);
                */

                IF.OnModuleLoaded(module, new EventArgs());
                OnModuleLoaded(module, new EventArgs());
            }
            else
                throw new ArgumentException("this module is not registered!\nGuid: " + id.ToString("b"));
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
            OnModuleUnload(ActiveModule, new EventArgs());

            ActiveModule.Unload();
            ActiveModule = null;

            /*
             * moved to Mainwindow using event handler for IF.ModuleUnloaded event
            if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
            {
                ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                    ChameleonCoderApp.Gui.CurrentModule.Text = string.Empty));
            }
            else
                ChameleonCoderApp.Gui.CurrentModule.Text = string.Empty;
            */

            IF.OnModuleUnloaded(module, new EventArgs());
            OnModuleUnloaded(module, new EventArgs());
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
        public bool TryGetModule(Guid id, out ILanguageModule module)
        {
            return Modules.TryGetValue(id, out module);
        }

        /// <summary>
        /// checks if a module with a given identifier is registered
        /// </summary>
        /// <param name="id">the module's identifier</param>
        /// <returns>true if the module is registered, false otherwise</returns>
        public bool IsModuleRegistered(Guid id)
        {
            return Modules.ContainsKey(id);
        }

        /// <summary>
        /// gets a list with all registered modules
        /// </summary>
        /// <returns>a list with all registered modules</returns>
        public IEnumerable<ILanguageModule> GetModules()
        {
            return Modules.Values; // TODO: return clone?
        }

        #endregion

        #region IService

        [ComVisible(false)]
        ConcurrentDictionary<Guid, IService> Services = new ConcurrentDictionary<Guid, IService>();

        /// <summary>
        /// gets the count of registered services
        /// </summary>
        public int ServiceCount { get { return Services.Count; } }

        /// <summary>
        /// calls a service given its identifier
        /// </summary>
        /// <param name="id">the service's identifier</param>
        public void CallService(Guid id)
        {
            IService service = GetService(id);

            IF.OnServiceExecute(service, new EventArgs());
            OnServiceExecute(service, new EventArgs());

            /*
             * moved to Mainwindow using event handler for IF.ServiceExecute
            ChameleonCoderApp.Gui.CurrentActionProgress.IsIndeterminate = true;
            ChameleonCoderApp.Gui.CurrentAction.Text = string.Format(Properties.Resources.ServiceInfo, service.Name, service.Version, service.Author, service.About);
            */

            service.Execute();
            while (service.IsBusy)
                System.Threading.Thread.Sleep(100);

            /*
             * moved to Mainwindow using event handler for IF.ServiceExecuted
            ChameleonCoderApp.Gui.CurrentActionProgress.IsIndeterminate = false;
            ChameleonCoderApp.Gui.CurrentAction.Text = string.Empty;
            */

            IF.OnServiceExecuted(service, new EventArgs());
            OnServiceExecuted(service, new EventArgs());
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
            if (Services.TryGetValue(id, out service))
                return service;
            throw new ArgumentException("this service is not registered!\nGuid: " + id.ToString("b"));
        }

        /// <summary>
        /// gets a list with all registered services
        /// </summary>
        /// <returns>a list with all registered services</returns>
        public IEnumerable<IService> GetServices()
        {
            return Services.Values; // TODO: return clone?
        }

        #endregion

        #region ITemplate

        [ComVisible(false)]
        ConcurrentDictionary<Guid, ITemplate> Templates = new ConcurrentDictionary<Guid, ITemplate>();

        /// <summary>
        /// gets the count of registered templates
        /// </summary>
        public int TemplateCount { get { return Templates.Count; } }

        /// <summary>
        /// gets a list with all registered templates
        /// </summary>
        /// <returns>a list with all registered templates</returns>
        public IEnumerable<ITemplate> GetTemplates()
        {
            return Templates.Values;
        }

        #endregion

        #region IResourceFactory

        [ComVisible(false)]
        ConcurrentDictionary<Guid, IResourceFactory> ResourceFactories = new ConcurrentDictionary<Guid, IResourceFactory>();

        /// <summary>
        /// gets the count of registered IResourceFactory
        /// </summary>
        public int ResourceFactoryCount { get { return ResourceFactories.Count; } }

        /// <summary>
        /// gets a list of all registered IResourceFactory
        /// </summary>
        /// <returns>the list</returns>
        public IEnumerable<IResourceFactory> GetResourceFactories()
        {
            return ResourceFactories.Values; // TODO: return clone?
        }

        public bool IsResourceFactoryRegistered(IResourceFactory factory)
        {
            return ResourceFactories.Values.Contains(factory);
        }

        #endregion

        #region IRichContentFactory

        [ComVisible(false)]
        ConcurrentDictionary<Guid, IRichContentFactory> RichContentFactories = new ConcurrentDictionary<Guid, IRichContentFactory>();

        /// <summary>
        /// gets the count of registered IRichContentFactory
        /// </summary>
        public int RichContentFactoryCount { get { return RichContentFactories.Count; } }

        /// <summary>
        /// gets a list of all registered IRichContentFactory
        /// </summary>
        /// <returns>the list</returns>
        public IEnumerable<IRichContentFactory> GetRichContentFactories()
        {
            return RichContentFactories.Values; // TODO: return clone?
        }

        public bool IsRichContentFactoryRegistered(IRichContentFactory factory)
        {
            return RichContentFactories.Values.Contains(factory);
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

        #endregion

        #region event infrastructure

        /// <summary>
        /// raises the ModuleLoad event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        [ComVisible(false)]
        internal void OnModuleLoad(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoad;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleLoaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        [ComVisible(false)]
        internal void OnModuleLoaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleUnload event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        [ComVisible(false)]
        internal void OnModuleUnload(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnload;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleUnloaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        [ComVisible(false)]
        internal void OnModuleUnloaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnloaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ServiceExecute event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        [ComVisible(false)]
        internal void OnServiceExecute(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecute;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ServiceExecuted event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        [ComVisible(false)]
        internal void OnServiceExecuted(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecuted;
            if (handler != null)
                handler(sender, e);
        }

        #endregion
    }
}
