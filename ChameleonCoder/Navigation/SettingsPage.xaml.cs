using System;
using System.Windows.Controls;


namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        internal SettingsPage()
        {
            DataContext = App.Gui.DataContext;
            InitializeComponent();

            langCombo.SelectedItem = Properties.Resources.Culture.LCID;

            ignoreChecking = true;
            extInstCheck.IsChecked = (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccm") != null
                    && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null);
        }

        bool ignoreChecking = false;

        private void InstallExtensions(object sender, EventArgs e)
        {
            if (ignoreChecking)
                ignoreChecking = false;
            else
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(App.AppPath, "--install_ext") { UseShellExecute = true, Verb = "runAs" };
                try { System.Diagnostics.Process.Start(info); }
                catch (System.ComponentModel.Win32Exception) { }
            }
        }

        private void SetLanguage(object sender, EventArgs e)
        {
            int LCID = (int)langCombo.SelectedItem;
            Properties.Resources.Culture = new System.Globalization.CultureInfo(LCID);
            Properties.Settings.Default.Language = LCID;
            Properties.Settings.Default.Save();
        }
    }
}
