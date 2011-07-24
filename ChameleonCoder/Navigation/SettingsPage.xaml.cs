using System;
using System.Windows.Controls;


namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            extInstCheck.SetCurrentValue(CheckBox.IsCheckedProperty, true);
            //extInstCheck.IsChecked = (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccm") != null
            //        && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null);
        }

        private void InstallExtensions(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(App.AppPath, "--install_ext") { UseShellExecute = true, Verb = "runAs" };
            try { System.Diagnostics.Process.Start(info); }
            catch (System.ComponentModel.Win32Exception) { }
        }
    }
}
