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

            extInstCheck.IsChecked = (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccp") != null
                    && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null);
        }

        private void InstallExtensions(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(App.AppPath, "--install_ext") { UseShellExecute = true, Verb = "runAs" };
            try { System.Diagnostics.Process.Start(info); }
            catch (System.ComponentModel.Win32Exception) { }
        }
    }
}
