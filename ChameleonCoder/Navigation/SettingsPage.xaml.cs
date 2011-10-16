namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the app settings
    /// </summary>
    internal sealed partial class SettingsPage : System.Windows.Controls.Page
    {
        internal SettingsPage()
        {
            ModelClientHelper.InitializeModel(ViewModel.SettingsPageModel.Instance);

            DataContext = ViewModel.SettingsPageModel.Instance;
            CommandBindings.AddRange(ViewModel.SettingsPageModel.Instance.Commands);

            InitializeComponent();
        }
    }
}
