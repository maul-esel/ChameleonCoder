using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the app settings
    /// </summary>
    internal sealed partial class SettingsPage : Page
    {
        internal SettingsPage()
        {
            DataContext = App.Gui.DataContext;
            InitializeComponent();

            langCombo.SelectedItem = Properties.Resources.Culture.LCID;

            extInstCheck.IsChecked = (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccp") != null
                    && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null);
        }

        private void InstallExtensions(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(App.AppPath, "--install_ext") { UseShellExecute = true, Verb = "runAs" };
            try { System.Diagnostics.Process.Start(info); }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void SetLanguage(object sender, EventArgs e)
        {
            Properties.Resources.Culture =
                new System.Globalization.CultureInfo(
                    Properties.Settings.Default.Language = (int)langCombo.SelectedItem
                    );

            ViewModel model = new ViewModel() { Tabs = App.Gui.MVVM.Tabs };
            App.Gui.DataContext = model;
            App.Gui.breadcrumb.Path = App.Gui.breadcrumb.PathFromBreadcrumbItem(App.Gui.breadcrumb.RootItem) + "/" + model.Item_Settings;

            Interaction.InformationProvider.OnLanguageChanged();
        }
    }
}
