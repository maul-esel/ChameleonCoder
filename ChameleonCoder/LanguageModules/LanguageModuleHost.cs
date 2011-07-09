using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;
using System.Linq;
using System.Text;

namespace ChameleonCoder.LanguageModules
{
    internal sealed class LanguageModuleHost : ILanguageModuleHost
    {
        #region infrastructure

        private static SortedList<Guid, ILanguageModule> Modules = new SortedList<Guid, ILanguageModule>();
        private static Guid ActiveModule = Guid.Empty;
        private static LanguageModuleHost instance = new LanguageModuleHost();

        internal static void Add(Type type)
        {
            ILanguageModule module = Activator.CreateInstance(type) as ILanguageModule;
            if (module != null)
            {
                Modules.Add(module.Language, module);
                module.Initialize(instance as ILanguageModuleHost);
            }
        }

        internal static void Shutdown()
        {
            UnloadModule();
            foreach (ILanguageModule module in Modules.Values)
                module.Shutdown();
        }

        internal static void UnloadModule()
        {
            if (ActiveModule != Guid.Empty)
            {
                ILanguageModule module;
                if (Modules.TryGetValue(ActiveModule, out module))
                {
                    module.Unload();
                    App.Gui.CustomGroup1.Items.Clear();
                    App.Gui.CustomGroup2.Items.Clear();
                    App.Gui.CustomGroup3.Items.Clear();
                }
                ActiveModule = Guid.Empty;
                App.Gui.CurrentModule.Text = string.Empty;
            }
        }

        internal static void LoadModule(Guid language)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(language, out module))
            {
                module.Load();
                ActiveModule = language;

                App.Gui.CurrentModule.Text = string.Format(Localization.Language.ModuleInfo,
                    module.LanguageName, module.Version, module.Author);
            }
        }

        internal static ILanguageModule GetModule(Guid language)
        {
            ILanguageModule module;
            if (Modules.TryGetValue(language, out module))
                return module;
            return null;
        }

        #endregion

        #region ILanguageModuleHost

        int ILanguageModuleHost.MinSupportedAPIVersion { get { return 1; } }
        int ILanguageModuleHost.MaxSupportedAPIVersion { get { return 1; } }

        void ILanguageModuleHost.RegisterTool()
        {
        }

        void ILanguageModuleHost.RegisterCodeGenerator(Func<Guid, string> clicked, ImageSource image)
        {
        }

        void ILanguageModuleHost.RegisterStubCreator(int index, Action<Guid> clicked)
        {
        }

        void ILanguageModuleHost.RegisterStubCreator(ImageSource custom, Action<Guid> clicked)
        {
        }

        void ILanguageModuleHost.RegisterEditTool()
        {
        }
        
        /*void ILanguageModuleHost.AddButton(string text, System.Windows.Media.ImageSource Image, System.Windows.RoutedEventHandler clickHandler, int Panel)
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
        }*/

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

        #region IPluginHost

        void IPluginHost.AddMetadata(Guid resource, string name, string value)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddMetadata(Guid resource, string name, string value, bool isDefault, bool noconfig)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(IResource resource)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(IResource resource, Guid parent)
        {
            throw new NotImplementedException();
        }

        Guid IPluginHost.GetCurrentResource()
        {
            return ChameleonCoder.Resources.Management.ResourceManager.ActiveItem.GUID;
        }

        int IPluginHost.GetCurrentView()
        {
            throw new NotImplementedException();
        }

        IResource IPluginHost.GetResource(Guid ID)
        {
            return ChameleonCoder.Resources.Management.ResourceManager.FlatList.GetInstance(ID);
        }
        #endregion

    }
}
