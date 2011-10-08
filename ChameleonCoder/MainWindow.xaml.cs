using System;
using System.Windows;
using ChameleonCoder.Navigation;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Shared;
using ChameleonCoder.ViewModel;
using ChameleonCoder.ViewModel.Interaction;
using Odyssey.Controls;
using MVVM = ChameleonCoder.ViewModel.MainWindowModel;

namespace ChameleonCoder
{
    /// <summary>
    /// the main window displaying the IDE
    /// </summary>
    internal sealed partial class MainWindow : RibbonWindow
    {
        internal MainWindow()
        {
            MVVM.Instance.Report -= ReportMessage;
            MVVM.Instance.Report += ReportMessage;

            MVVM.Instance.Confirm -= ConfirmMessage;
            MVVM.Instance.Confirm += ConfirmMessage;

            MVVM.Instance.UserInput -= GetInput;
            MVVM.Instance.UserInput += GetInput;

            MVVM.Instance.ViewChanged -= AdjustView;
            MVVM.Instance.ViewChanged += AdjustView;

            MVVM.Instance.RepresentationNeeded -= GetRepresentation;
            MVVM.Instance.RepresentationNeeded += GetRepresentation;

            DataContext = MVVM.Instance;
            CommandBindings.AddRange(MVVM.Instance.Commands);
            InitializeComponent();

            ChameleonCoderCommands.OpenNewTab.Execute(null, this);         
        }

        #region view model interaction

        /// <summary>
        /// reports a message by the view model to the user
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private static void ReportMessage(object sender, ReportEventArgs e)
        {
            MessageBoxImage icon;
            switch (e.Severity)
            {
                case ViewModel.Interaction.MessageSeverity.Error:
                    icon = MessageBoxImage.Error;
                    break;

                case ViewModel.Interaction.MessageSeverity.Critical:
                    icon = MessageBoxImage.Exclamation;
                    break;

                default:
                case ViewModel.Interaction.MessageSeverity.Information:
                    icon = MessageBoxImage.Information;
                    break;
            }

            MessageBox.Show(e.Message, e.Topic, MessageBoxButton.OK, icon);
        }

        /// <summary>
        /// confirms a message by the view model by letting the user decide
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private static void ConfirmMessage(object sender, ConfirmationEventArgs e)
        {
            e.Accepted = MessageBox.Show(e.Message,
                                        e.Topic,
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        /// <summary>
        /// gets user input for the view model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private static void GetInput(object sender, UserInputEventArgs e)
        {
            var box = new InputBox(e.Topic, e.Message);
            if (box.ShowDialog() == true)
                e.Input = box.Text;
        }

        /// <summary>
        /// reacts to a change on the internal view (i.e. the loaded page / tab) of the model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private void AdjustView(object sender, ViewChangedEventArgs e)
        {
            ribbon.ContextualTabSet = null;
            ribbon.SelectedTabItem = ribbon.Tabs[0];

            switch (e.NewView.Type)
            {
                case CCTabPage.Home:
                case CCTabPage.Plugins:
                case CCTabPage.Settings:
                case CCTabPage.FileManagement:

                    if (ResourceManager.ActiveItem != null)
                        ResourceManager.Close();
                    break;

                case CCTabPage.ResourceList:

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[0];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];

                    if (ResourceManager.ActiveItem != null)
                        ResourceManager.Close();
                    break;

                case CCTabPage.ResourceView:

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[2];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];
                    break;

                case CCTabPage.ResourceEdit:

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[1];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];
                    break;

                default:
                case CCTabPage.None:
                    throw new InvalidOperationException("page type is not known");
            }
        }

        /// <summary>
        /// gets a view for a given view model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private static void GetRepresentation(object sender, RepresentationEventArgs e)
        {
            if (e.Model is WelcomePageModel)
            {
                e.Representation = new WelcomePage();
            }
            else if (e.Model is ResourceListPageModel)
            {
                e.Representation = new ResourceListPage();
            }
            else if (e.Model is SettingsPageModel)
            {
                e.Representation = new SettingsPage();
            }
            else if (e.Model is PluginPageModel)
            {
                e.Representation = new PluginPage();
            }
            else if (e.Model is FileManagementPageModel)
            {
                e.Representation = new FileManagementPage(e.Model as FileManagementPageModel);
            }
            else if (e.Model is ResourceViewPageModel)
            {
                e.Representation = new ResourceViewPage(e.Model as ResourceViewPageModel);
            }
            else if (e.Model is EditPageModel)
            {
                e.Representation = new EditPage(e.Model as EditPageModel);
            }
        }

        #endregion

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Input.NavigationCommands.Refresh.Execute(null, MVVM.Instance.ActiveTab.Content as IInputElement);
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (IsInitialized)
                ChameleonCoderCommands.SetGroupingMode.Execute((sender as RibbonToggleButton).IsChecked == true,
                    MVVM.Instance.ActiveTab.Content as IInputElement);
        }

        private void SortingChanged(object sender, EventArgs e)
        {
            if (IsInitialized)
                ChameleonCoderCommands.SetSortingMode.Execute((sender as RibbonToggleButton).IsChecked == true,
                    MVVM.Instance.ActiveTab.Content as IInputElement);
        }

        #region resources

        [Obsolete("to be moved to model", false)]
        private void ResourceCopy(object sender, EventArgs e)
        {
            ResourceSelector selector = new ResourceSelector(1);
            if (selector.ShowDialog() == true
                && selector.ResourceList.Count > 0
                && selector.ResourceList[0] != ResourceManager.ActiveItem)
            {
                ResourceManager.ActiveItem.Copy(selector.ResourceList[0]);
            }
        }

        [Obsolete("to be moved to model", false)]
        private void ResourceMove(object sender, EventArgs e)
        {
            ResourceSelector selector = new ResourceSelector(1);
            if (selector.ShowDialog() == true // user did not cancel
                && selector.ResourceList.Count > 0 // user selected 1 resource
                && selector.ResourceList[0] != null // resource is not null
                && selector.ResourceList[0] != ResourceManager.ActiveItem.Parent) // resource is not already parent
            {
                if (!selector.ResourceList[0].IsDescendantOf(ResourceManager.ActiveItem)) // can't be moved to descendant
                {
                    ResourceManager.ActiveItem.Move(selector.ResourceList[0]);
                }
                else
                    MessageBox.Show(string.Format(Properties.Resources.Error_MoveToDescendant, ResourceManager.ActiveItem.Name, selector.ResourceList[0].Name),
                                    Properties.Resources.Status_Move);
            }
        }

        [Obsolete("to be replaced by command", false)]
        private void ResourceOpen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IResource resource = TreeView.SelectedItem as IResource;
            if (resource == null)
            {
                var reference = TreeView.SelectedItem as Resources.ResourceReference;
                if (reference == null)
                    return;
                resource = reference.Resolve();
            }
            ResourceOpen(resource);
        }

        [Obsolete("to be moved to model", false)]
        private void ResourceOpen(object sender, RoutedPropertyChangedEventArgs<BreadcrumbItem> e)
        {
            if (e.NewValue != null)
            {
                BreadcrumbContext context = e.NewValue.DataContext as BreadcrumbContext;

                if (context != null)
                {
                    switch (context.PageType)
                    {
                        case Shared.CCTabPage.Home:
                            System.Windows.Input.NavigationCommands.BrowseHome.Execute(null, this);
                            break;

                        case Shared.CCTabPage.ResourceList:
                            ChameleonCoderCommands.OpenResourceListPage.Execute(null, this);
                            break;

                        case Shared.CCTabPage.Settings:
                            ChameleonCoderCommands.OpenSettingsPage.Execute(null, this);
                            break;

                        case Shared.CCTabPage.Plugins:
                            ChameleonCoderCommands.OpenPluginPage.Execute(null, this);
                            break;

                        case Shared.CCTabPage.FileManagement:
                            ChameleonCoderCommands.OpenFileManagementPage.Execute(App.DefaultFile, this);
                            break;
                    }
                }
                else
                    ResourceOpen(ResourceHelper.GetResourceFromPath(breadcrumb.PathFromBreadcrumbItem(e.NewValue), breadcrumb.SeparatorString));
            }
        }

        [Obsolete("to be moved to model", false)]
        private void ResourceOpen(IResource resource)
        {
            ChameleonCoderCommands.OpenResourceView.Execute(resource, this);
        }
        #endregion
    }
}
