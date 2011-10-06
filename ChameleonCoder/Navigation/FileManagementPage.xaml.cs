namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Shared logic for FileManagementPage.xaml
    /// </summary>
    internal sealed partial class FileManagementPage : CCPageBase
    {
        internal FileManagementPage(ViewModel.FileManagementPageModel model)
        {
            Initialize(model);
            InitializeComponent();
        }
    }
}
