using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// a dialog for selecting plugins to install and installing them
    /// </summary>
    public partial class PluginInstaller : Window
    {
        public PluginInstaller(List<IPlugin> plugins)
        {
            pluginList = plugins;
            DataContext = new { plugins = plugins, lang = new ViewModel() };
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
        }

        private void Install(IEnumerable<IPlugin> plugins)
        {
            List<Type> pluginTypes = new List<Type>();

            foreach (IPlugin plugin in plugins)
            {
                Properties.Settings.Default.InstalledPlugins.Add(plugin.Identifier.ToString("n"));
                pluginTypes.Add(plugin.GetType());
                pluginList.Remove(plugin);

                if (Path.GetDirectoryName(plugin.GetType().Assembly.Location) != Path.Combine(App.AppDir, "Components"))
                    File.Copy(plugin.GetType().Assembly.Location,
                        Path.Combine(App.AppDir, "Components\\",
                        Path.GetFileName(plugin.GetType().Assembly.Location)));
            }

            PluginManager.Load(pluginTypes);

            DataContext = new { plugins = pluginList, lang = new ViewModel() };
            Properties.Settings.Default.Save();
        }

        List<IPlugin> pluginList;
    }
}
