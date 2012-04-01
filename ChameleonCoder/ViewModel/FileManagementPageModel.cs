using System.Collections.Generic;
using System.Windows.Input;
using ChameleonCoder.Files;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(Navigation.FileManagementPage))]
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

            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddReference,
                AddReferenceCommandExecuted,
                (s, e) => e.CanExecute = ActiveFile != null));

            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteReference,
                DeleteReferenceCommandExecuted,
                (s, e) => e.CanExecute = ActiveFile != null && ActiveReference != null));
        }

        public static IList<DataFile> AllFiles
        {
            get
            {
                return ChameleonCoderApp.RunningObject.FileManager.Files;
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
                    return ActiveFile.GetReferences();                
                return null;
            }
        }

        public object ActiveReference
        {
            get;
            set;
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

        private void AddReferenceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            AddReference(e.Parameter as string == "file" ? DataFileReferenceType.File : DataFileReferenceType.Directory);
        }

        private void DeleteReferenceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            if (ActiveFile != null && ActiveReference != null)
            {
                ActiveFile.DeleteReference(((DataFileReference)ActiveReference).Identifier);
                OnPropertyChanged("References");
                OnPropertyChanged("ActiveReference");
            }
        }

        #endregion

        private void AddReference(DataFileReferenceType type)
        {
            if (ActiveFile != null)
            {
                string path = null;

                switch (type)
                {
                    case DataFileReferenceType.File:
                        var fileArgs = OnReferenceFileNeeded(Res.Status_CreatingReference + " " + Res.Ref_SelectTarget, true);
                        if (fileArgs.Cancel)
                            return;

                        path = fileArgs.Path;
                        if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
                        {
                            OnReport(Res.Status_CreatingReference, string.Format(Res.Error_InvalidFile, path), Interaction.MessageSeverity.Critical);
                            return;
                        }
                        break;
                    case DataFileReferenceType.Directory:
                        var directoryArgs = OnReferenceDirectoryNeeded(Res.Status_CreatingReference + " " + Res.Ref_SelectTarget, true);
                        if (directoryArgs.Cancel)
                            return;

                        path = directoryArgs.Path;
                        if (string.IsNullOrWhiteSpace(path) || !System.IO.Directory.Exists(path))
                        {
                            OnReport(Res.Status_CreatingReference, string.Format(Res.Error_NonExistentDir, path), Interaction.MessageSeverity.Critical);
                            return;
                        }
                        break;
                    default:
                        throw new System.NotImplementedException();
                }

                ActiveFile.AddReference(path, type);
                OnPropertyChanged("References");
            }
        }

        private void AddMetadata()
        {
            if (ActiveFile != null)
            {
                var args = OnUserInput(Res.Status_CreateMeta, Res.Meta_EnterName);
                if (args.Cancel)
                    return;

                var name = args.Input;
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

        #region events

        internal event System.EventHandler<Interaction.FileSelectionEventArgs> ReferenceFileNeeded;

        private Interaction.FileSelectionEventArgs OnReferenceFileNeeded(string message, bool mustExist)
        {
            var handler = ReferenceFileNeeded;

            if (handler != null)
            {
                var args = new Interaction.FileSelectionEventArgs(message, System.Environment.CurrentDirectory, "CC Resource files | *.ccr", mustExist);
                handler(this, args);
                return args;
            }

            return null;
        }

        internal event System.EventHandler<Interaction.DirectorySelectionEventArgs> ReferenceDirectoryNeeded;

        private Interaction.DirectorySelectionEventArgs OnReferenceDirectoryNeeded(string message, bool allowCreate)
        {
            var handler = ReferenceDirectoryNeeded;

            if (handler != null)
            {
                var args = new Interaction.DirectorySelectionEventArgs(message, System.Environment.CurrentDirectory, allowCreate);
                handler(this, args);
                return args;
            }

            return null;
        }

        #endregion
    }
}
