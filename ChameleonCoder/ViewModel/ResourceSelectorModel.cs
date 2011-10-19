using System.Collections.Generic;
using System.Windows.Input;
using ChameleonCoder.Resources;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(Shared.ResourceSelector))]
    internal sealed class ResourceSelectorModel : ViewModelBase
    {
        internal ResourceSelectorModel(ResourceCollection resources, int count)
        {
            Commands.Add(new CommandBinding(ChameleonCoderCommands.AddResource,
                AddResourceCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.RemoveResource,
                RemoveResourceCommandExecuted));

            resourceList = resources;
            maxResourceCount = count;
        }

        #region commanding

        private void AddResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var resource = e.Parameter as ChameleonCoder.Resources.Interfaces.IResource;
            var reference = e.Parameter as ResourceReference;
            SelectedResources.Add(resource != null ? (IComponent)resource : reference);

            NotifyActiveResourceChanged();
        }

        private void RemoveResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var resource = e.Parameter as ChameleonCoder.Resources.Interfaces.IResource;
            var reference = e.Parameter as ResourceReference;
            SelectedResources.Remove(resource != null ? (IComponent)resource : reference);

            NotifyActiveResourceChanged();
        }

        #endregion

        public IComponent ActiveResource
        {
            get
            {
                return OnActiveResourceNeeded();
            }
        }

        public bool ShowReferences
        {
            get { return showReferenceValue; }
            set
            {
                showReferenceValue = value;
                OnPropertyChanged("ShowReferences");
            }
        }

        public ResourceCollection Resources
        {
            get { return resourceList; }
        }

        public IList<IComponent> SelectedResources
        {
            get { return selectedResourceList; }
        }

        public bool CanAdd
        {
            get
            {
                var resource = OnActiveResourceNeeded();
                return (resource != null
                    && !SelectedResources.Contains(resource)
                    && (SelectedResources.Count < maxResourceCount || maxResourceCount == -1));
            }
        }

        public bool CanRemove
        {
            get
            {
                var resource = OnActiveResourceNeeded();
                return (resource != null
                    && SelectedResources.Contains(resource));
            }
        }

        private int maxResourceCount;

        private readonly ResourceCollection resourceList;

        private readonly IList<IComponent> selectedResourceList = new List<IComponent>();

        private bool showReferenceValue = true;

        #region localization

        public static string Title { get { return MainWindowModel.Title; } }

        public static string Info_Name { get { return Res.Info_Name; } }

        public static string Info_GUID { get { return Res.Info_Identifier; } }

        public static string Info_Icon { get { return Res.Info_Icon; } }

        public static string Info_Description { get { return Res.Info_Description; } }

        public static string Info_Special { get { return Res.Info_Special; } }

        public static string Action_Add { get { return Res.Action_Add; } }

        public static string Action_Remove { get { return Res.Action_Remove; } }

        public static string Action_Cancel { get { return Res.Action_Cancel; } }

        public static string Action_OK { get { return Res.Action_OK; } }

        #endregion

        #region events

        public event System.EventHandler<Interaction.InformationEventArgs<IComponent>> ActiveResourceNeeded;

        private IComponent OnActiveResourceNeeded()
        {
            var handler = ActiveResourceNeeded;

            if (handler != null)
            {
                var args = new Interaction.InformationEventArgs<IComponent>();
                handler(this, args);
                return args.Information;
            }

            return null;
        }

        #endregion

        public void NotifyActiveResourceChanged()
        {
            OnPropertyChanged("ActiveResource");
            OnPropertyChanged("CanAdd");
            OnPropertyChanged("CanRemove");
        }
    }
}
