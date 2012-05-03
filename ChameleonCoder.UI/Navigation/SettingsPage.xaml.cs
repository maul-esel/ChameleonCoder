namespace ChameleonCoder.UI.Navigation
{
    /// <summary>
    /// a page displaying the app settings
    /// </summary>
    internal sealed partial class SettingsPage : System.Windows.Controls.Page
    {
        internal SettingsPage(ViewModel.SettingsPageModel model)
        {
            ModelClientHelper.InitializeModel(model);

            DataContext = model;
            CommandBindings.AddRange(model.Commands);

            InitializeComponent();
        }
    }
}
