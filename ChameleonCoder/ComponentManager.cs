using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ChameleonCoder.LanguageModules;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.RichContent;
using ChameleonCoder.Services;
using IF = ChameleonCoder.Interaction.InformationProvider;

namespace ChameleonCoder
{
    public static class ComponentManager
    {
        internal static void TryAdd(Type component)
        {
            if (component.GetConstructor(Type.EmptyTypes) == null)
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

        internal static ILanguageModule ActiveModule { get; private set; }

        internal static int ModuleCount { get { return Modules.Count; } }


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

        public static ILanguageModule GetModule(Guid id)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(id, out module))
                return module;
            throw new ArgumentException("this module is not registered!\nGuid: " + id.ToString("b"));
        }

        public static bool TryGetModule(Guid id, out ILanguageModule module)
        {
            return Modules.TryGetValue(id, out module);
        }

        public static IEnumerable<ILanguageModule> GetModules()
        {
            return Modules.Values;
        }

        #endregion

        #region IService

        static ConcurrentDictionary<Guid, IService> Services = new ConcurrentDictionary<Guid, IService>();

        internal static int ServiceCount { get { return Services.Count; } }


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

        internal static IService GetService(Guid id)
        {
            IService service;
            if (Services.TryGetValue(id, out service))
                return service;
            throw new ArgumentException("this service is not registered!\nGuid: " + id.ToString("b"));
        }

        internal static IEnumerable<IService> GetServices()
        {
            return Services.Values;
        }

        #endregion

        #region ITemplate

        static ConcurrentDictionary<Guid, ITemplate> Templates = new ConcurrentDictionary<Guid, ITemplate>();

        static int TemplateCount { get { return Templates.Count; } }


        internal static void Create(Guid id, Resources.Interfaces.IResource parent)
        {
            ITemplate template;
            if (Templates.TryGetValue(id, out template))
            {
                template.Create(parent);
            }
        }

        #endregion
    }
}
