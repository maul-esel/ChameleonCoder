using System;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Shared;
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
            ModelClientHelper.InitializeModel(MVVM.Instance);

            MVVM.Instance.ViewChanged -= AdjustView;
            MVVM.Instance.ViewChanged += AdjustView;

            MVVM.Instance.SelectFile -= ModelClientHelper.SelectFile;
            MVVM.Instance.SelectFile += ModelClientHelper.SelectFile;

            DataContext = MVVM.Instance;
            CommandBindings.AddRange(MVVM.Instance.Commands);

            InitializeComponent();

            ChameleonCoderCommands.OpenNewTab.Execute(null, this);
        }

        #region view model interaction

        /// <summary>
        /// reacts to a change on the internal view (i.e. the loaded page / tab) of the model
        /// </summary>
        /// <param name="sender">the view model sending the event</param>
        /// <param name="e">additional data related to the event</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private void AdjustView(object sender, ViewEventArgs e)
        {
            ribbon.ContextualTabSet = null;
            ribbon.SelectedTabItem = ribbon.Tabs[0];

            switch (e.NewView.Type)
            {
                case CCTabPage.Home:
                case CCTabPage.Plugins:
                case CCTabPage.Settings:

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

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[1];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];
                    break;

                case CCTabPage.ResourceEdit:

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[2];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];
                    break;

                case CCTabPage.FileManagement:

                    if (ResourceManager.ActiveItem != null)
                        ResourceManager.Close();

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[3];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];

                    break;

                default:
                case CCTabPage.None:
                    throw new InvalidOperationException("page type is not known");
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
                            ChameleonCoderCommands.OpenFileManagementPage.Execute(ChameleonCoderApp.DefaultFile, this);
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
