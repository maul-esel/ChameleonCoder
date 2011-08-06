using System;
using System.Windows.Controls;
using System.Windows.Data;
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
            DataContext = App.Gui.DataContext;
            InitializeComponent();
        }

        private void OpenResourceList(object sender, EventArgs e)
        {
            App.Gui.GoList(null, null);
        }

        private void CreateResource(object sender, EventArgs e)
        {
            NewResourceDialog dialog = new NewResourceDialog();
            dialog.ShowDialog();
        }

        private void OpenConfiguration(object sender, EventArgs e)
        {
            App.Gui.GoSettings(null, null);
        }
    }
}
