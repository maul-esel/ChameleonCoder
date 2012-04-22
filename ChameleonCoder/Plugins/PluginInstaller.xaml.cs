using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// a dialog for selecting plugins to install and installing them
    /// </summary>
    internal sealed partial class PluginInstaller : Window
    {
        internal PluginInstaller(ViewModel.PluginInstallerModel model)
        {
            DataContext = this.model = model;
            pluginList = model.PluginList;
            InitializeComponent();
        }

        private void InstallSelected(object sender, EventArgs e)
        {
            List<IPlugin> plugins = new List<IPlugin>();

            foreach (IPlugin plugin in list.Items)
            {
                var item = list.ItemContainerGenerator.ContainerFromItem(plugin);

                for (int i = 0; i < 6; i++)
                    item = VisualTreeHelper.GetChild(item, i == 3 ? 1 : 0);

                if ((item as CheckBox).IsChecked == true)
                    plugins.Add(plugin); 
            }

            Install(plugins);
        }

        private void InstallAll(object sender, EventArgs e)
        {
            List<IPlugin> plugins = new List<IPlugin>();

            foreach (IPlugin plugin in list.Items)
                plugins.Add(plugin);

            Install(plugins);

            Close();
        }

        private void Install(IEnumerable<IPlugin> plugins) // todo: move to PluginManager + also move InformationProvider events there
        {
            List<Type> pluginTypes = new List<Type>();

            foreach (IPlugin plugin in plugins)
            {
                Settings.ChameleonCoderSettings.Default.InstalledPlugins.Add(plugin.Identifier.ToString("n"));
                pluginTypes.Add(plugin.GetType());
                pluginList.Remove(plugin);

                if (Path.GetDirectoryName(plugin.GetType().Assembly.Location) != Path.Combine(ChameleonCoderApp.AppDir, "Components"))
                    File.Copy(plugin.GetType().Assembly.Location,
                        Path.Combine(ChameleonCoderApp.AppDir, "Components\\",
                        Path.GetFileName(plugin.GetType().Assembly.Location)));

                Shared.InformationProvider.OnPluginInstalled(plugin);
            }

            model.App.PluginMan.Load(pluginTypes);
        }

        private readonly ObservableCollection<IPlugin> pluginList = null;
        private readonly ViewModel.PluginInstallerModel model = null;
    }
}
