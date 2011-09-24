using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a Page displaying all registered plugins
    /// </summary>
    internal sealed partial class PluginPage : Page
    {
        /// <summary>
        /// creates a new instance of the page
        /// </summary>
        public PluginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// removes the selected plugin from the list of installed plugins. On next launch, it won't be loaded.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void Uninstall(object sender, EventArgs e)
        {
            var plugin = list.SelectedItem as IPlugin;
            Settings.ChameleonCoderSettings.Default.InstalledPlugins.Remove(plugin.Identifier.ToString("n"));

            Update();
        }

        /// <summary>
        /// lets the user select an assembly and plugin classes inside it
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void Install(object sender, EventArgs e)
        {
            string path = null;

            using (var dialog = new System.Windows.Forms.OpenFileDialog() { Filter = "plugins | *.dll" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = dialog.FileName;
                }
            }

            if (path != null)
            {
                Assembly ass;

                try { ass = Assembly.LoadFrom(path); }
                catch (BadImageFormatException ex)
                {
                    MessageBox.Show(string.Format(Properties.Resources.Error_InstallNoAssembly, path),
                        Properties.Resources.Status_InstallPlugin);
                    App.Log(GetType().ToString() + " --> private void Install(object, EventArgs)",
                        "catched exception: *.dll could not be loaded (" + path + ").",
                        ex.ToString());
                    return;
                }

                if (!Attribute.IsDefined(ass, typeof(CCPluginAttribute)))
                {
                    MessageBox.Show(string.Format(Properties.Resources.Error_InstallNoPlugin, path),
                        Properties.Resources.Status_InstallPlugin);
                    App.Log(GetType().ToString() + " --> private void Install(object, EventArgs)",
                        "refused plugin install: Assembly does not have CCPluginAttribute defined (" + path + ").",
                        null);
                    return;
                }

                var types = from type in ass.GetTypes()
                            where Attribute.IsDefined(type, typeof(CCPluginAttribute))
                                && !type.IsValueType && !type.IsAbstract && type.IsClass && type.IsPublic
                                && type.GetInterface(typeof(Plugins.IPlugin).FullName) != null
                                && type.GetConstructor(Type.EmptyTypes) != null
                            select type;

                List<IPlugin> newPlugins = new List<IPlugin>();
                foreach (var component in types)
                {
                    IPlugin plugin = Activator.CreateInstance(component) as IPlugin;
                    if (!Settings.ChameleonCoderSettings.Default.InstalledPlugins.Contains(plugin.Identifier.ToString("n")))
                        newPlugins.Add(plugin);
                }

                if (newPlugins.Count == 0)
                {
                    MessageBox.Show(string.Format(Properties.Resources.Error_InstallEmptyAssembly, path),
                        Properties.Resources.Status_InstallPlugin);
                    App.Log(GetType().ToString() + " --> private void Install(object, EventArgs)",
                        "refused plugin install: Assembly does not contain plugin classes that aren't already installed (" + path + ").",
                        null);
                    return;
                }

                var installer = new PluginInstaller(newPlugins);
                installer.ShowDialog();

                Update();
            }
        }

        /// <summary>
        /// updates the filter
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void RefreshFilter(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();
        }

        /// <summary>
        /// filters the listboxitems to fit the type selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter(object sender, FilterEventArgs e)
        {
            if (IsInitialized && categories.SelectedIndex != -1)
            {
                int category = categories.SelectedIndex;

                if (category == 0) // if all plugins should be shown:
                    e.Accepted = true; // accept everything
                else
                {
                    if (e.Item is ITemplate) // compare selected category with required one
                        e.Accepted = category == 1;
                    if (e.Item is IService)
                        e.Accepted = category == 2;
                    if (e.Item is ILanguageModule)
                        e.Accepted = category == 3;
                    if (e.Item is IResourceFactory)
                        e.Accepted = category == 4;
                }
            }
        }

        private IList<IPlugin> FilterPlugins(IEnumerable<IPlugin> plugins)
        {
            var list = new List<IPlugin>();
            foreach (var plugin in plugins)
            {
                if (Settings.ChameleonCoderSettings.Default.InstalledPlugins.Contains(plugin.Identifier.ToString("n")))
                    list.Add(plugin);
            }
            return list;
        }

        [Obsolete("needs to be update: no re-instantiating", false)]
        private void Update()
        {
            DataContext = new ViewModel.PluginPageModel(FilterPlugins(PluginManager.GetPlugins()));
        }
    }
}
