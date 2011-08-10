using System;
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
            var plugins = Plugins.PluginManager.GetPlugins(true);
            DataContext = new { Lang = new ViewModel(), Plugins = plugins };
            InitializeComponent();
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
        /// disables a plugin
        /// </summary>
        /// <param name="sender">the button raising the event</param>
        /// <param name="e">additional data</param>
        private void Disable(object sender, EventArgs e)
        {
            IPlugin plugin = GetPlugin(sender as FrameworkElement);

            if (!Properties.Settings.Default.DisabledPlugins.Contains(plugin.Identifier.ToString("n"))) // if already disabled, ignore it
                Properties.Settings.Default.DisabledPlugins.Add(plugin.Identifier.ToString("n")); // otherwise add it to the list
            Properties.Settings.Default.Save(); // save the settings

            // todo:
            // * instant visual update: re-apply template or similar
            // * instant PluginManager modification: remove from corresponding list,
            //      add to list of disabled plugins,
            //      if ComponentFactory: remove registered types
        }

        /// <summary>
        /// enables a previously disabled plugin
        /// </summary>
        /// <param name="sender">the button raising the event</param>
        /// <param name="e">additional data</param>
        private void Enable(object sender, EventArgs e)
        {
            IPlugin plugin = GetPlugin(sender as FrameworkElement);

            if (Properties.Settings.Default.DisabledPlugins.Contains(plugin.Identifier.ToString("n"))) // check: if included in list
                Properties.Settings.Default.DisabledPlugins.Remove(plugin.Identifier.ToString("n")); // if so, remove it
            Properties.Settings.Default.Save(); // save updated settings

            // todo:
            // * instant visual update
            // * initialize the plugin, move it to the corresponding PluginManager list
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
            if (IsInitialized && categories.SelectedItem != null)
            {
                string category = (categories.SelectedItem as ListBoxItem).Content as string;

                if (category == App.Gui.MVVM.Item_Plugins) // if all plugins should be shown:
                    e.Accepted = true; // accept everything
                else
                {
                    if (e.Item is ILanguageModule) // compare selected category with required one
                        e.Accepted = category == App.Gui.MVVM.Plugin_LanguageModule;
                    if (e.Item is IService)
                        e.Accepted = category == App.Gui.MVVM.Plugin_Service;
                    if (e.Item is ITemplate)
                        e.Accepted = category == App.Gui.MVVM.Plugin_Template;
                    if (e.Item is IComponentFactory)
                        e.Accepted = category == App.Gui.MVVM.Plugin_ComponentFactory;
                }
            }
        }
    }
}
