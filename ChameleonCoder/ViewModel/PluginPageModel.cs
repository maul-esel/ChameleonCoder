using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChameleonCoder.Plugins;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the PluginPage class
    /// </summary>
    internal sealed class PluginPageModel : ViewModelBase
    {
        internal PluginPageModel(IList<IPlugin> pluginList)
        {
            plugins = new ObservableCollection<IPlugin>(pluginList);
        }

        public ObservableCollection<IPlugin> PluginList
        {
            get { return plugins; }
        }

        private readonly ObservableCollection<IPlugin> plugins;

        public static string Types_All { get { return Res.PP_TypesAll; } }

        public static string Types_Templates { get { return Res.PP_TypesTemplate; } }

        public static string Types_Services { get { return Res.PP_TypesService; } }

        public static string Types_LanguageModules { get { return Res.PP_TypesLanguageModule; } }

        public static string Types_ResourceFactories { get { return Res.PP_TypesResourceFactory; } }

        public static string Uninstall { get { return Res.PP_Uninstall; } }

        public static string Install { get { return Res.PP_Install; } }
    }
}
