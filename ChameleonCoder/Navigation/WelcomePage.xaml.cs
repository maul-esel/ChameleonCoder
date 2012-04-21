namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a welcome page displaying several buttons
    /// </summary>
    internal sealed partial class WelcomePage : System.Windows.Controls.Page
    {
        internal WelcomePage(ViewModel.WelcomePageModel model)
        {
            ModelClientHelper.InitializeModel(model);
            DataContext = model;
            CommandBindings.AddRange(model.Commands);
            InitializeComponent();
        }
    }
}
