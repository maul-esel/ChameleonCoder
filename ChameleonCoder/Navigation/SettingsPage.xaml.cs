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
            this.DataContext = this;
            InitializeComponent();
        }

        public bool IsExtensionsInstalled
        {
            get
            {
                return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccm") != null
                    && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null;
            }
            set
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(App.AppPath, "--install_ext") { UseShellExecute = true };
                info.Verb = "runAs";
                System.Diagnostics.Process.Start(info); 
            }
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
