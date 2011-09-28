using System.Collections.ObjectModel;
using ChameleonCoder.Plugins;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class PluginInstallerModel : ViewModelBase
    {
        internal PluginInstallerModel(ObservableCollection<IPlugin> plugins)
        {
            this.plugins = plugins;
        }

        public ObservableCollection<IPlugin> PluginList
        {
            get { return plugins; }
        }

        private readonly ObservableCollection<IPlugin> plugins;

        #region localization

        public string Cancel { get { return Res.Action_Cancel; } }

        public string InstallSelected { get { return Res.Plugin_InstallSelected; } }

        public string InstallAll { get { return Res.Plugin_InstallAll; } }

        #endregion
    }
}
