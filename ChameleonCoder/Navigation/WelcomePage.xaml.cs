namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a welcome page displaying several buttons
    /// </summary>
    internal sealed partial class WelcomePage : System.Windows.Controls.Page
    {
        internal WelcomePage()
        {
            ModelClientHelper.InitializeModel(ViewModel.WelcomePageModel.Instance);

            DataContext = ViewModel.WelcomePageModel.Instance;
            CommandBindings.AddRange(ViewModel.WelcomePageModel.Instance.Commands);

            InitializeComponent();
        }
    }
}
