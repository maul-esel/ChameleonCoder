using System.Collections.Generic;
using System.Windows.Input;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class FileManagementPageModel : ViewModelBase
    {
        internal FileManagementPageModel(DataFile file)
        {
            this.dataFile = file;

            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteMetadata,
                DeleteMetadataCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddMetadata,
                AddMetadataCommandExecuted));
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
                return dataFile.GetMetadata();
            }
        }

        public DataFile File
        {
            get { return dataFile; }
        }

        private readonly DataFile dataFile;

        public object ActiveMetadata
        {
            get;
            set;
        }

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

            if (ActiveMetadata != null)
            {
                var key = ((KeyValuePair<string, string>)ActiveMetadata).Key;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    dataFile.DeleteMetadata(key);
                    OnPropertyChanged("Metadata");
                }
            }
        }

        #endregion

        private void AddMetadata()
        {
            var name = OnUserInput(Res.Status_CreateMeta, Res.Meta_EnterName);

            if (string.IsNullOrWhiteSpace(name))
            {
                OnReport(Res.Status_CreateMeta, Res.Error_MetaInvalidName, Interaction.MessageSeverity.Error);
                return;
            }
            else if (dataFile.GetMetadata(name) != null)
            {
                OnReport(Res.Status_CreateMeta, Res.Error_MetaDuplicateName, Interaction.MessageSeverity.Error);
                return;
            }

            dataFile.SetMetadata(name, null);
            OnPropertyChanged("Metadata");
        }
    }
}
