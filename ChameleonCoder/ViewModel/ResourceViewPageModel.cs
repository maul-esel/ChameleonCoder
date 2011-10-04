using System.Collections.Generic;
using System.Windows.Input;
using ChameleonCoder.Resources.Interfaces;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// a class containing localization strings and other data for the ResourceViewPage class
    /// </summary>
    internal sealed class ResourceViewPageModel : ViewModelBase
    {
        internal ResourceViewPageModel(IResource resource)
        {
            resourceInstance = resource;

            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteMetadata,
                DeleteMetadataCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddMetadata,
                AddMetadataCommandExecuted));
        }

        #region resource & properties
        public IResource Resource { get { return resourceInstance; } }

        private readonly IResource resourceInstance;

        public IDictionary<string, string> Metadata { get { return resourceInstance.GetMetadata(); } }

        public object ActiveMetadata
        {
            get;
            set;
        }
        #endregion

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
                    Resource.DeleteMetadata(key);
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
            else if (Resource.GetMetadata(name) != null)
            {
                OnReport(Res.Status_CreateMeta, Res.Error_MetaDuplicateName, Interaction.MessageSeverity.Error);
                return;
            }

            Resource.SetMetadata(name, null);
            OnPropertyChanged("Metadata");
        }
    }
}
