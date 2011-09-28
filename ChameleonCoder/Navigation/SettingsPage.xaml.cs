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
            DataContext = ViewModel.SettingsPageModel.Instance;
            InitializeComponent();
        }
    }
}
