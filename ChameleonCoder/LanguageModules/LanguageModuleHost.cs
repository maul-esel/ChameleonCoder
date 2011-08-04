using System;
using System.Collections.Generic;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.LanguageModules
{
    public static class LanguageModuleHost
    {
        private static SortedList<Guid, ILanguageModule> Modules = new SortedList<Guid, ILanguageModule>();
        private static Guid ActiveModule = Guid.Empty;

        internal static void Add(Type type)
        {
            ILanguageModule module = Activator.CreateInstance(type) as ILanguageModule;
            if (module != null)
            {
                Modules.Add(module.Language, module);
                module.Initialize();
            }
        }

        internal static void Shutdown()
        {
            UnloadModule();
            foreach (ILanguageModule module in Modules.Values)
                if (module != null)
                    module.Shutdown();
        }

        internal static void UnloadModule()
        {
            if (ActiveModule != Guid.Empty)
            {
                ILanguageModule module;
                if (Modules.TryGetValue(ActiveModule, out module))
                {
                    Interaction.InformationProvider.OnModuleUnload(module, new EventArgs());

                    module.Unload();
                    if (App.Gui != null)
                    {
                        App.Gui.CustomGroup1.Controls.Clear();
                        App.Gui.CustomGroup2.Controls.Clear();
                        App.Gui.CustomGroup3.Controls.Clear();
                        App.Gui.CurrentModule.Text = string.Empty;
                    }

                    ActiveModule = Guid.Empty;

                    Interaction.InformationProvider.OnModuleUnloaded(module, new EventArgs());
                }
                
            }
        }

        public static ILanguageModule GetActiveModule()
        {
            return GetModule(ActiveModule);
        }

        internal static void LoadModule(Guid language)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(language, out module))
            {
                Interaction.InformationProvider.OnModuleLoad(module, new EventArgs());

                module.Load();
                ActiveModule = language;

                App.Gui.CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                    module.LanguageName, module.Version, module.Author, module.About);

                Interaction.InformationProvider.OnModuleLoaded(module, new EventArgs());
            }
        }

        static object lock_getmodule = new object();
        public static ILanguageModule GetModule(Guid language)
        {
            lock (lock_getmodule)
            {
                ILanguageModule module;
                if (Modules.TryGetValue(language, out module))
                    return module;
            }
            return null;
        }

        public static IEnumerable<ILanguageModule> GetList()
        {
            return Modules.Values;
        }
    }
}
