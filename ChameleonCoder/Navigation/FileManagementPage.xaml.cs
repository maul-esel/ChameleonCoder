using Forms = System.Windows.Forms;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Shared logic for FileManagementPage.xaml
    /// </summary>
    internal sealed partial class FileManagementPage : CCPageBase
    {
        internal FileManagementPage(ViewModel.FileManagementPageModel model)
        {
            model.ReferenceFileNeeded -= GetFile;
            model.ReferenceFileNeeded += GetFile;

            model.ReferenceDirectoryNeeded -= GetDirectory;
            model.ReferenceDirectoryNeeded += GetDirectory;

            Initialize(model);
            InitializeComponent();
        }

        private void GetFile(object sender, ViewModel.Interaction.FileSelectionEventArgs e)
        {
            using (var dialog = new Forms.OpenFileDialog() { Filter = e.Filter,
                                                             Title = e.Message,
                                                             CheckPathExists = e.MustExist,
                                                             InitialDirectory = e.Directory })
            {
                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    e.Path = dialog.FileName;
                }
            }
        }

        private void GetDirectory(object sender, ViewModel.Interaction.DirectorySelectionEventArgs e)
        {
            using (var dialog = new Forms.FolderBrowserDialog() { Description = e.Message,
                                                                  SelectedPath = e.InitialDirectory,
                                                                  ShowNewFolderButton = e.AllowCreation })
            {
                if (dialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    e.Path = dialog.SelectedPath;
                }
            }
        }
    }
}
