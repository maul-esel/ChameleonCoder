using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChameleonCoder.Plugins
{
    internal class PluginHost : ILanguageModuleHost, IServiceHost
    {
        #region IPluginHost

        void IPluginHost.AddMetadata(Guid resource, string name, string value)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddMetadata(Guid resource, string name, string value, bool isDefault, bool noconfig)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(Resources.Base.ResourceBase resource)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(Resources.Base.ResourceBase resource, Guid parent)
        {
            throw new NotImplementedException();
        }

        Guid IPluginHost.GetCurrentResource()
        {
            return ChameleonCoder.Resources.Base.ResourceManager.ActiveItem.GUID;
        }

        int IPluginHost.GetCurrentView()
        {
            throw new NotImplementedException();
        }

        Resources.Base.ResourceBase IPluginHost.GetResource(Guid ID)
        {
            return ChameleonCoder.Resources.Base.ResourceManager.FlatList.GetInstance(ID);
        }

        System.Windows.Window IPluginHost.GetWindow()
        {
            return App.Gui;
        }

        #endregion

        #region ILanguageModuleHost

        int ILanguageModuleHost.MinSupportedAPIVersion { get { return 1; } }
        int ILanguageModuleHost.MaxSupportedAPIVersion { get { return 1; } }

        void ILanguageModuleHost.AddButton(string text, System.Windows.Media.ImageSource Image, System.Windows.RoutedEventHandler clickHandler, int Panel)
        {
            Microsoft.Windows.Controls.Ribbon.RibbonButton button = new Microsoft.Windows.Controls.Ribbon.RibbonButton();
            
            button.Click += clickHandler;
            button.Label = text;
            button.LargeImageSource = Image;

            switch (Panel)
            { 
                case 1: App.Gui.CustomGroup1.Items.Add(button); break;
                case 2: App.Gui.CustomGroup2.Items.Add(button); break;
                case 3: App.Gui.CustomGroup3.Items.Add(button); break;
                default: throw new InvalidOperationException("invalid ribbon panel number: " + Panel);
            }
        }

        string ILanguageModuleHost.GetCurrentEditText()
        {
            return App.Gui.Editor.Text;
        }

        ICSharpCode.AvalonEdit.TextEditor ILanguageModuleHost.GetEditControl()
        {
            return App.Gui.Editor;
        }

        void ILanguageModuleHost.InsertCode(string code)
        {
            // App.Gui.Editor.Document.Insert(0, code);
            // todo: find current cursor position
            throw new NotImplementedException();
        }

        void ILanguageModuleHost.InsertCode(string code, int position)
        {
            // should check for current view before
            App.Gui.Editor.Document.Insert(position, code);
        }

        #endregion

        #region IServiceHost

        int IServiceHost.MinSupportedAPIVersion { get { return 1; } }

        int IServiceHost.MaxSupportedAPIVersion { get { return 1; } }

        #endregion

        #region infrastructure

        private SortedList<Guid, ILanguageModule> LanguageModules = new SortedList<Guid, ILanguageModule>();

        private SortedList<Guid, IService> Services = new SortedList<Guid, IService>();

        private Guid ActiveLanguageModule;

        internal PluginHost()
        {
            App.Current.Exit += PluginHost.Shutdown;

            // code from http://dotnet-snippets.de/dns/c-search-plugin-dlls-with-one-line-SID1089.aspx, slightly modified
            var result = from dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Plugins", "*.dll")
                         let a = Assembly.LoadFrom(dll)
                         from t in a.GetTypes()
                         where t.GetInterface(typeof(ILanguageModule).ToString()) != null
                         select Activator.CreateInstance(t) as ILanguageModule;
            // ... up to here ;-)

            foreach (ILanguageModule module in result)
            {
                if (module != null)
                    LanguageModules.Add(module.Language, module);
            }

            foreach (ILanguageModule module in LanguageModules.Values)
            {
                module.Initialize(this as ILanguageModuleHost);
            }

            // code from http://dotnet-snippets.de/dns/c-search-plugin-dlls-with-one-line-SID1089.aspx, slightly modified
            var result2 = from dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Plugins", "*.dll")
                         let a = Assembly.LoadFrom(dll)
                         from t in a.GetTypes()
                         where t.GetInterface(typeof(IService).ToString()) != null
                         select Activator.CreateInstance(t) as IService;
            // ... up to here ;-)

            foreach (IService service in result2)
            {
                if (service != null)
                    Services.Add(service.Service, service);
            }

            foreach (IService service in Services.Values)
            {
                service.Initialize(this as IServiceHost);
            }
        }

        internal static void Shutdown(object sender, EventArgs e)
        {
            App.Host.UnloadModule();
            foreach (ILanguageModule module in App.Host.LanguageModules.Values)
            {
                module.Shutdown();
            }
            foreach (IService service in App.Host.Services.Values)
            {
                service.Shutdown();
            }
        }

        internal void UnloadModule()
        {
            if (ActiveLanguageModule != null)
            {
                ILanguageModule module;
                if (this.LanguageModules.TryGetValue(ActiveLanguageModule, out module))
                {
                    module.Unload();
                    App.Gui.CustomGroup1.Items.Clear();
                    App.Gui.CustomGroup2.Items.Clear();
                    App.Gui.CustomGroup3.Items.Clear();
                }
            }
        }

        internal void LoadModule(Guid language)
        {
            ILanguageModule module;
            if (this.LanguageModules.TryGetValue(language, out module))
            {
                module.Load();
                ActiveLanguageModule = language;
            }
        }

        #endregion
    }
}
