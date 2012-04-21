using System.Collections.ObjectModel;
using ChameleonCoder.Plugins;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(PluginInstaller))]
    internal sealed class PluginInstallerModel : ViewModelBase
    {
        internal PluginInstallerModel(ChameleonCoderApp app, ObservableCollection<IPlugin> plugins)
            : base(app)
        {
            this.plugins = plugins;
        }

        public ObservableCollection<IPlugin> PluginList
        {
            get { return plugins; }
        }

        private readonly ObservableCollection<IPlugin> plugins;

        #region localization

        public static string Cancel { get { return Res.Action_Cancel; } }

        public static string InstallSelected { get { return Res.PI_InstallSelected; } }

        public static string InstallAll { get { return Res.PI_InstallAll; } }

        #endregion
    }
}
