using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the app settings
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        internal SettingsPage()
        {
            DataContext = App.Gui.DataContext;
            InitializeComponent();

            langCombo.SelectedItem = Properties.Resources.Culture.LCID;

            extInstCheck.IsChecked = (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccp") != null
                    && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null);
            codeFont.SelectedItem = new System.Windows.Media.FontFamily(Properties.Settings.Default.CodeFont);
        }

        private void InstallExtensions(object sender, EventArgs e)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(App.AppPath, "--install_ext") { UseShellExecute = true, Verb = "runAs" };
            try { System.Diagnostics.Process.Start(info); }
            catch (System.ComponentModel.Win32Exception) { }
        }

        private void SetLanguage(object sender, EventArgs e)
        {
            int LCID = (int)langCombo.SelectedItem;
            Properties.Resources.Culture = new System.Globalization.CultureInfo(LCID);
            Properties.Settings.Default.Language = LCID;
            Properties.Settings.Default.Save();

            ViewModel model = new ViewModel() { Tabs = App.Gui.MVVM.Tabs };
            App.Gui.DataContext = model;
            App.Gui.breadcrumb.Path = App.Gui.breadcrumb.PathFromBreadcrumbItem(App.Gui.breadcrumb.RootItem) + "/" + model.Item_Settings;

            Interaction.InformationProvider.OnLanguageChanged();
        }

        public static FontFamily CodeFont
        {
            get
            {
                return family;
            }

            set
            {
                family = value;
                Properties.Settings.Default.CodeFont = value.Source;
            }
        }

        private static FontFamily family = new FontFamily(Properties.Settings.Default.CodeFont);
    }
}
