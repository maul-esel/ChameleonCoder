using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void OpenResourceView(object sender, EventArgs e)
        {
            int i = App.Gui.FindPageTab(typeof(ResourceListPage));

            if (i == -1)
                i = App.Gui.Tabs.Items.Add(new KeyValuePair<string, Page>("resources", new ResourceListPage()));

            App.Gui.Tabs.SelectedIndex = i; 
        }

        private void CreateResource(object sender, EventArgs e)
        {
            ResourceTypeSelector selector = new ResourceTypeSelector();

            if (selector.ShowDialog() == true)
                ResourceTypeManager.GetInfo(selector.SelectedResult).Creator(selector.SelectedResult, null, ResourceManager.Add);
        }

        private void OpenConfiguration(object sender, EventArgs e)
        {
            int i = App.Gui.FindPageTab(typeof(SettingsPage));

            if (i == -1)
                i = App.Gui.Tabs.Items.Add(new KeyValuePair<string, Page>("settings", new SettingsPage()));
            App.Gui.Tabs.SelectedIndex = i;
        }
    }
}
