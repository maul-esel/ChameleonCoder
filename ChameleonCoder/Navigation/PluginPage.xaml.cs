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
    public partial class PluginPage : Page
    {
        /// <summary>
        /// creates a new instance of the page
        /// </summary>
        public PluginPage()
        {
            DataContext = new KeyValuePair<ViewModel, IEnumerable<IPlugin>>(new ViewModel(),
                FilterPlugins(Plugins.PluginManager.GetPlugins()));
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
            Properties.Settings.Default.InstalledPlugins.Remove(plugin.Identifier.ToString("n"));

            DataContext = new KeyValuePair<ViewModel,IEnumerable<IPlugin>>(new ViewModel(),
                FilterPlugins(Plugins.PluginManager.GetPlugins()));
        }

        /// <summary>
        /// lets the user select an assembly and plugin classes inside it
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void Install(object sender, EventArgs e)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog() { Filter = "plugins | *.dll" };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.FileName;
                Assembly ass;

                dialog.Dispose();

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
                    if (!Properties.Settings.Default.InstalledPlugins.Contains(plugin.Identifier.ToString("n")))
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

                DataContext = new KeyValuePair<ViewModel, IEnumerable<IPlugin>>(new ViewModel(),
                    FilterPlugins(Plugins.PluginManager.GetPlugins()));
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
        /// gets the plugin from a button on the representing ListBoxItem
        /// </summary>
        /// <param name="ctrl">the button</param>
        /// <returns>the plugin</returns>
        private IPlugin GetPlugin(FrameworkElement ctrl)
        {
            for (int i = 0; i < 2; i++)
                ctrl = ctrl.Parent as FrameworkElement;
            return ctrl.DataContext as IPlugin;
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
                    if (e.Item is IComponentFactory)
                        e.Accepted = category == 4;
                }
            }
        }

        private IList<IPlugin> FilterPlugins(IEnumerable<IPlugin> plugins)
        {
            var list = new List<IPlugin>();
            foreach (var plugin in plugins)
            {
                if (Properties.Settings.Default.InstalledPlugins.Contains(plugin.Identifier.ToString("n")))
                    list.Add(plugin);
            }
            return list;
        }
    }
}
