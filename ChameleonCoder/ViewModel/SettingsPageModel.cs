using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the SettingsPage class
    /// </summary>
    [DefaultRepresentation(typeof(Navigation.SettingsPage))]
    internal sealed class SettingsPageModel : ViewModelBase
    {
        private SettingsPageModel()
        {
        }

        internal static SettingsPageModel Instance
        {
            get
            {
                lock (modelInstance)
                {
                    return modelInstance;
                }
            }
        }

        private static readonly SettingsPageModel modelInstance = new SettingsPageModel();

        [NotifyParentProperty(false)]
        public static int[] availableTranslations { get { return new int[2] { 1031, 1033 }; } }

        public FontFamily CodeFont
        {
            get
            {
                return family;
            }
            set
            {
                Settings.ChameleonCoderSettings.Default.CodeFont = (family = value).Source;
                OnPropertyChanged("CodeFont");
            }
        }

        private static FontFamily family = new FontFamily(Settings.ChameleonCoderSettings.Default.CodeFont);

        public int CodeFontSize
        {
            get { return Settings.ChameleonCoderSettings.Default.CodeFontSize; }
            set
            {
                Settings.ChameleonCoderSettings.Default.CodeFontSize = value;
                OnPropertyChanged("CodeFontSize");
            }
        }

        [NotifyParentProperty(false)]
        public int UILanguage
        {
            get { return Settings.ChameleonCoderSettings.Default.Language; }

            set
            {
                Res.Culture =
                    new System.Globalization.CultureInfo(
                        Settings.ChameleonCoderSettings.Default.Language = value
                        );

                Shared.InformationProvider.OnLanguageChanged();

                // HACK: this should be done in main window or its model, not here
                var breadcrumb = App.Gui.breadcrumb;

                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem)
                                + breadcrumb.SeparatorString + Item_Settings;

                OnPropertyChanged("UILanguage");
            }
        }

        public bool ExtInstalled
        {
            get { return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null; }

            set
            {
                var param = (value) ? "--install_ext" : "--uninstall_ext";
                var info = new ProcessStartInfo(ChameleonCoderApp.AppPath, param) { Verb = "runAs" };
                try
                {
                    using (var process = Process.Start(info))
                    {
                        process.WaitForExit();
                    }
                }
                catch (System.ComponentModel.Win32Exception)
                {
                }
                OnPropertyChanged("ExtInstalled");
            }
        }

        public static string Item_Settings { get { return Res.Item_Settings; } }

        public static string Setting_Language { get { return Res.SP_Language; } }

        public static string Setting_EnableUpdate { get { return Res.SP_Update; } }

        public static string Setting_InstallExt { get { return Res.SP_InstallExt; } }

        public static string Setting_InstallCOM { get { return Res.SP_InstallCOM; } }

        public static string Setting_CodeFont { get { return Res.SP_CodeFont; } }

        public static string Setting_CodeFontSize { get { return Res.SP_CodeFontSize; } }
    }
}
