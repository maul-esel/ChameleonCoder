namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the app settings
    /// </summary>
    internal sealed partial class SettingsPage : CCPageBase
    {
        internal SettingsPage()
            : base(ViewModel.SettingsPageModel.Instance)
        {
            InitializeComponent();
        }
    }
}
