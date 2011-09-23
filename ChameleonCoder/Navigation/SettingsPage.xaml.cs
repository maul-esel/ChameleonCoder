using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the app settings
    /// </summary>
    internal sealed partial class SettingsPage : Page
    {
        internal SettingsPage()
        {
            InitializeComponent();
            Update();

            extInstCheck.IsChecked = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null;
        }

        private void InstallExtensions(object sender, EventArgs e)
        {
            var info = new ProcessStartInfo(App.AppPath, "--install_ext") { Verb = "runAs" };
            try
            {
                using (var process = Process.Start(info)) { process.WaitForExit(); }
            }
            catch (System.ComponentModel.Win32Exception)
            {
            }
            extInstCheck.IsChecked = (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null);
        }

        private void Update()
        {
            DataContext = new ViewModel.SettingsPageModel();
        }
    }
}
