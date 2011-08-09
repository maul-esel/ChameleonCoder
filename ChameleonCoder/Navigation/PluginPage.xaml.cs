using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für PluginPage.xaml
    /// </summary>
    public partial class PluginPage : Page
    {
        public PluginPage()
        {
            var plugins = Plugins.PluginManager.GetPlugins();
            DataContext = new { Lang = new ViewModel(), Plugins = plugins };
            InitializeComponent();
        }

        private void RefreshFilter(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();
        }

        private void Disable(object sender, EventArgs e)
        {
            FrameworkElement ctrl = sender as FrameworkElement;
            for (int i = 0; i < 2; i++)
                ctrl = ctrl.Parent as FrameworkElement;
            IPlugin plugin = ctrl.DataContext as IPlugin;

            Properties.Settings.Default.DisabledPlugins.Add(plugin.Identifier.ToString("n"));
            Properties.Settings.Default.Save();
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            if (IsInitialized && categories.SelectedItem != null)
            {
                string category = (categories.SelectedItem as ListBoxItem).Content as string;

                if (category == App.Gui.MVVM.Item_Plugins)
                    e.Accepted = true;
                else
                {
                    if (e.Item is ILanguageModule)
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
