namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a welcome page displaying several buttons
    /// </summary>
    internal sealed partial class WelcomePage : CCPageBase
    {
        /// <summary>
        /// creates a new instance of this page
        /// </summary>
        internal WelcomePage()
            : base(ViewModel.WelcomePageModel.Instance)
        {
            InitializeComponent();
        }
    }
}
