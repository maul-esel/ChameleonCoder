namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a welcome page displaying several buttons
    /// </summary>
    internal sealed partial class WelcomePage : System.Windows.Controls.Page
    {
        /// <summary>
        /// creates a new instance of this page
        /// </summary>
        internal WelcomePage()
        {
            DataContext = ViewModel.WelcomePageModel.Instance;
            InitializeComponent();
        }
    }
}
