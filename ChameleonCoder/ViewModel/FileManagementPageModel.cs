using System.Collections.Generic;
using System.Windows.Input;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class FileManagementPageModel : ViewModelBase
    {
        internal FileManagementPageModel()
        {
            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteMetadata,
                DeleteMetadataCommandExecuted,
                (s, e) => e.CanExecute = ActiveFile != null && ActiveMetadata != null));

            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddMetadata,
                AddMetadataCommandExecuted,
                (s, e) => e.CanExecute = ActiveFile != null));
        }

        public static IList<DataFile> AllFiles
        {
            get
            {
                return DataFile.LoadedFiles;
            }
        }

        public IDictionary<string, string> Metadata
        {
            get
            {
                if (ActiveFile != null)
                    return ActiveFile.GetMetadata();
                return null;
            }
        }

        public IList<DataFileReference> References
        {
            get
            {
                if (ActiveFile != null)
                    return ActiveFile.References;                
                return null;
            }
        }

        public DataFile ActiveFile
        {
            get { return file; }
            set
            {
                file = value;                
                OnPropertyChanged("ActiveFile");
                UpdateAll();
            }
        }

        private DataFile file;

        public object ActiveMetadata
        {
            get { return metadata; }
            set
            {
                metadata = value;
                OnPropertyChanged("ActiveMetadata");
            }
        }

        private object metadata;

        #region localization

        public static string MetadataKey { get { return Res.MetadataKey; } }

        public static string MetadataValue { get { return Res.MetadataValue; } }

        #endregion

        #region commanding

        private void AddMetadataCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            AddMetadata();
        }

        private void DeleteMetadataCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            if (ActiveMetadata != null && ActiveFile != null)
            {
                var key = ((KeyValuePair<string, string>)ActiveMetadata).Key;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    ActiveFile.DeleteMetadata(key);
                    OnPropertyChanged("Metadata");
                }
            }
        }
        #endregion

        private void AddMetadata()
        {
            if (ActiveFile != null)
            {
                var name = OnUserInput(Res.Status_CreateMeta, Res.Meta_EnterName);

                if (string.IsNullOrWhiteSpace(name))
                {
                    OnReport(Res.Status_CreateMeta, Res.Error_MetaInvalidName, Interaction.MessageSeverity.Error);
                    return;
                }
                else if (ActiveFile.GetMetadata(name) != null)
                {
                    OnReport(Res.Status_CreateMeta, Res.Error_MetaDuplicateName, Interaction.MessageSeverity.Error);
                    return;
                }

                ActiveFile.SetMetadata(name, null);
                OnPropertyChanged("Metadata");
            }
        }
    }
}
