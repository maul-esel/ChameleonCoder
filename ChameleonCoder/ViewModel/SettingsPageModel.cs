using System;
using System.ComponentModel;
using System.Windows.Media;
using Res = ChameleonCoder.Properties.Resources;
using System.Diagnostics;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the SettingsPage class
    /// </summary>
    internal sealed class SettingsPageModel : ViewModelBase
    {
        [NotifyParentProperty(false)]
        public static int[] availableTranslations { get { return new int[2] { 1031, 1033 }; } }

        public FontFamily CodeFont
        {
            get { return family; }
            set { Settings.ChameleonCoderSettings.Default.CodeFont = (family = value).Source; }
        }

        private static FontFamily family = new FontFamily(Settings.ChameleonCoderSettings.Default.CodeFont);

        public static int CodeFontSize
        {
            get { return Settings.ChameleonCoderSettings.Default.CodeFontSize; }
            set { Settings.ChameleonCoderSettings.Default.CodeFontSize = value; }
        }

        [NotifyParentProperty(false)]
        public static int UILanguage
        {
            get { return Settings.ChameleonCoderSettings.Default.Language; }

            set
            {
                Res.Culture =
                    new System.Globalization.CultureInfo(
                        Settings.ChameleonCoderSettings.Default.Language = value
                        );

                Interaction.InformationProvider.OnLanguageChanged();

                var breadcrumb = App.Gui.breadcrumb;

                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem)
                                + breadcrumb.SeparatorString + Item_Settings;
            }
        }

        public bool ExtInstalled
        {
            get { return Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null; }

            set
            {
                var info = new ProcessStartInfo(App.AppPath, "--install_ext") { Verb = "runAs" };
                try
                {
                    using (var process = Process.Start(info)) { process.WaitForExit(); }
                }
                catch (System.ComponentModel.Win32Exception)
                {
                }
                Update("ExtInstalled");
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
