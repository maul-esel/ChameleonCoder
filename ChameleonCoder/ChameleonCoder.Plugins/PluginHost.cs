using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace ChameleonCoder.Plugins
{
    [Export(typeof(IPluginHost))]
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

        [ImportMany]
        IEnumerable<Lazy<ILanguageModule>> modules;

        public PluginHost()
        {
            AggregateCatalog PluginCatalog = new AggregateCatalog();
            PluginCatalog.Catalogs.Add(new AssemblyCatalog(typeof(PluginHost).Assembly));
            PluginCatalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Plugins"));
            
            CompositionContainer Container = new CompositionContainer(PluginCatalog);
            Container.ComposeParts(this);

            foreach (Lazy<ILanguageModule> i in this.modules)
                i.Value.Initalize();
        }
    }
}
