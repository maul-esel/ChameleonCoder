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
        internal WelcomePage()
        {
            InitializeComponent();
        }

        private void OpenResourceView(object sender, EventArgs e)
        {
            App.Gui.GoList(null, null);
        }

        private void CreateResource(object sender, EventArgs e)
        {
            Interaction.ResourceTypeSelector selector = new Interaction.ResourceTypeSelector();

            if (selector.ShowDialog() == true)
                ResourceTypeManager.GetInfo(selector.SelectedResult).Creator(selector.SelectedResult, null, ResourceManager.Add);
        }

        private void OpenConfiguration(object sender, EventArgs e)
        {
            App.Gui.GoSettings(null, null);
        }
    }
}
