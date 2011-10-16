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

            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddMetadata,
                AddMetadataCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteMetadata,
                DeleteMetadataCommandExecuted,
                (s, e) => e.CanExecute = ActiveMetadata != null));            

            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddReference,
                AddReferenceCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteReference,
                DeleteReferenceCommandExecuted,
                (s, e) => e.CanExecute = ActiveReference != null));
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

        /*
         * TODO:
         * - how to set this?
         *      - add reference list to page??
         *      - use context menu somehow?
         */
        public Resources.ResourceReference ActiveReference
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

        private void AddReferenceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            AddReference();
        }

        private void DeleteReferenceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            DeleteReference(ActiveReference);
        }

        #endregion

        private void AddMetadata()
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
            else if (Resource.GetMetadata(name) != null)
            {
                OnReport(Res.Status_CreateMeta, Res.Error_MetaDuplicateName, Interaction.MessageSeverity.Error);
                return;
            }

            Resource.SetMetadata(name, null);
            OnPropertyChanged("Metadata");
        }

        private void AddReference()
        {
            var model = new ResourceSelectorModel(Resources.Management.ResourceManager.GetChildren(), 1) { ShowReferences = false };
            OnRepresentationNeeded(model, true);

            if (model.SelectedResources.Count > 0)
            {
                var resource = model.SelectedResources[0] as IResource;
                if (resource != null)
                {
                    var args = OnUserInput(Res.Status_CreatingReference, Res.Ref_SelectTarget);
                    if (args.Cancel)
                        return;

                    var name = args.Input;

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        Resource.AddReference(name, resource.Identifier);
                    }
                }
            }
        }

        private void DeleteReference(Resources.ResourceReference reference)
        {
            if (reference != null)
            {
                Resource.DeleteReference(reference.Identifier);
            }
        }
    }
}
