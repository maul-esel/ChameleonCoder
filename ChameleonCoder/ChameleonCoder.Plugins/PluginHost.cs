using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChameleonCoder.Plugins
{
    class PluginHost : IPluginHost
    {
        int IPluginHost.MinSupportedAPIVersion { get { return 0; } }
        int IPluginHost.MaxSupportedAPIVersion { get { return 1; } }

        void IPluginHost.InsertCode(string code)
        {
            throw new System.NotImplementedException();
        }

        void IPluginHost.InsertCode(string code, int position)
        {
            throw new System.NotImplementedException();
        }

        void IPluginHost.AddButton(string text, System.Windows.Media.ImageSource Image, Action<object, EventArgs> clickHandler, int Panel)
        {
            throw new System.NotImplementedException();
        }

        void IPluginHost.AddMetadata(Guid resource, string name, string value)
        {
            throw new System.NotImplementedException();
        }

        void IPluginHost.AddMetadata(Guid resource, string name, string value, bool isDefault, bool noconfig)
        {
            throw new System.NotImplementedException();
        }

        static void Shutdown(object sender, EventArgs e)
        {
            foreach (ILanguageModule module in App.Host.LanguageModules.Values)
            {
                module.Shutdown();
            }
        }

        SortedList<Guid, ILanguageModule> LanguageModules = new SortedList<Guid, ILanguageModule>();

        public PluginHost()
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
                module.Initalize(this);
            }
            
        }
    }
}
