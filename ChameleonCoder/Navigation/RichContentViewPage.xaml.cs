namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaction logic for RichContentViewPage.xaml
    /// </summary>
    internal sealed partial class RichContentViewPage : System.Windows.Controls.Page
    {
        internal RichContentViewPage(string html)
        {
            InitializeComponent();
            NavigationService.Navigate(html);
        }
    }
}
