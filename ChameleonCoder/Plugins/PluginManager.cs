using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using IF = ChameleonCoder.Interaction.InformationProvider;

namespace ChameleonCoder.Plugins
{
    public static class PluginManager
    {
        /// <summary>
        /// tries adding a type to the corresponding plugin collections
        /// </summary>
        /// <param name="component"></param>
        internal static void TryAdd(Type component)
        {
            if (component.GetConstructor(Type.EmptyTypes) == null) // if no parameterless constructor: skip
                return;

            if (component.GetInterface(typeof(ITemplate).FullName) != null)
            {
                ITemplate template = Activator.CreateInstance(component) as ITemplate;
                if (template != null)
                {
                    Templates.TryAdd(template.Identifier, template);
                    template.Initialize();
                }
            }
            if (component.GetInterface(typeof(IService).FullName) != null)
            {
                IService service = Activator.CreateInstance(component) as IService;
                if (service != null)
                {
                    Services.TryAdd(service.Identifier, service);
                    service.Initialize();
                }
            }
            if (component.GetInterface(typeof(ILanguageModule).FullName) != null)
            {
                ILanguageModule module = Activator.CreateInstance(component) as ILanguageModule;
                if (module != null)
                {
                    Modules.TryAdd(module.Identifier, module);
                    module.Initialize();
                }
            }
        }

        /// <summary>
        /// calls IPlugin.Shutdown() on all plugins
        /// </summary>
        internal static void Shutdown()
        {
            foreach (ITemplate template in Templates.Values)
                template.Shutdown();
            foreach (IService service in Services.Values)
                service.Shutdown();
            foreach (ILanguageModule module in Modules.Values)
                module.Shutdown();
        }

        #region ILanguageModule

        static ConcurrentDictionary<Guid, ILanguageModule> Modules = new ConcurrentDictionary<Guid, ILanguageModule>();

        /// <summary>
        /// gets the currently loaded module
        /// </summary>
        internal static ILanguageModule ActiveModule { get; private set; }

        /// <summary>
        /// returns the count of language modules registered
        /// </summary>
        internal static int ModuleCount { get { return Modules.Count; } }

        /// <summary>
        /// loads a language module given its identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <exception cref="ArgumentException">thrown if no module with this identifier is registered.</exception>
        internal static void LoadModule(Guid id)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(id, out module))
            {
                lock (module)
                {
                    IF.OnModuleLoad(module, new EventArgs());

                    module.Load();
                    ActiveModule = module;

                    App.Gui.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        module.Name, module.Version, module.Author, module.About);

                    IF.OnModuleLoaded(module, new EventArgs());
                }
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
            lock (ActiveModule)
            {
                ILanguageModule module = ActiveModule;
                IF.OnModuleUnload(ActiveModule, new EventArgs());

                ActiveModule.Unload();
                ActiveModule = null;

                App.Gui.CurrentModule.Text = string.Empty;

                IF.OnModuleUnloaded(module, new EventArgs());
            }
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

            App.Gui.CurrentActionProgress.IsIndeterminate = true;
            App.Gui.CurrentAction.Text = string.Format(Properties.Resources.ServiceInfo, service.Name, service.Version, service.Author, service.About);

            service.Call();
            while (service.IsBusy)
                System.Threading.Thread.Sleep(100);

            App.Gui.CurrentActionProgress.IsIndeterminate = false;
            App.Gui.CurrentAction.Text = string.Empty;

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
        /// invokes the ITemplate.Create() method
        /// </summary>
        /// <param name="id">the template's identifier</param>
        /// <param name="parent">the parent resource for the new resource</param>
        /// <param name="name">the name for the new resource</param>
        internal static void Create(Guid id, Resources.Interfaces.IResource parent, string name)
        {
            ITemplate template;
            if (Templates.TryGetValue(id, out template))
            {
                template.Create(parent, name);
            }
        }

        /// <summary>
        /// gets a list with all registered templates
        /// </summary>
        /// <returns>a list with all registered templates</returns>
        internal static IEnumerable<ITemplate> GetTemplates()
        {
            return Templates.Values;
        }

        #endregion
    }
}
