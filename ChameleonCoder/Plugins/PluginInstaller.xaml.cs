using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private void Install(IEnumerable<IPlugin> plugins)
        {
            List<Type> pluginTypes = new List<Type>();

            foreach (IPlugin plugin in plugins)
            {
                model.App.PluginMan.InstallPermanently(plugin);
            }
        }

        private readonly ObservableCollection<IPlugin> pluginList = null;
        private readonly ViewModel.PluginInstallerModel model = null;
    }
}
