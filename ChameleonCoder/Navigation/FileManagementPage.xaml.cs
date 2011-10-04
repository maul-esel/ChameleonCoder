namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Shared logic for FileManagementPage.xaml
    /// </summary>
    internal sealed partial class FileManagementPage : CCPageBase
    {
        internal FileManagementPage(DataFile file)
        {
            Initialize(new ViewModel.FileManagementPageModel(file));
            InitializeComponent();
        }
    }
}
