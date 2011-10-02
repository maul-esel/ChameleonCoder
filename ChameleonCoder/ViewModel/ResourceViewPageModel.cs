using System.Collections.Generic;
using ChameleonCoder.Resources.Interfaces;
using System.Windows.Input;
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
        public static string MetadataKey { get { return Res.VP_MetadataKey; } }

        public static string MetadataValue { get { return Res.VP_MetadataValue; } }
        #endregion

        private void DeleteMetadataCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            if (ActiveMetadata != null)
            {
                var key = ((KeyValuePair<string, string>)ActiveMetadata).Key;
                if (!string.IsNullOrWhiteSpace(key))
                {
                    Resource.DeleteMetadata(key);
                    Update("Metadata");
                }
            }
        }
    }
}
