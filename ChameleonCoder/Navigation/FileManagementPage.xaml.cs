namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Shared logic for FileManagementPage.xaml
    /// </summary>
    internal sealed partial class FileManagementPage : System.Windows.Controls.Page
    {
        internal FileManagementPage(ViewModel.FileManagementPageModel model)
        {
            ModelClientHelper.InitializeModel(model);

            model.ReferenceFileNeeded -= ModelClientHelper.SelectFile;
            model.ReferenceFileNeeded += ModelClientHelper.SelectFile;

            model.ReferenceDirectoryNeeded -= ModelClientHelper.SelectDirectory;
            model.ReferenceDirectoryNeeded += ModelClientHelper.SelectDirectory;

            DataContext = model;
            CommandBindings.AddRange(model.Commands);

            InitializeComponent();
        }
    }
}
