using System;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Shared;
using ChameleonCoder.ViewModel.Interaction;
using Odyssey.Controls;

namespace ChameleonCoder
{
    /// <summary>
    /// the main window displaying the IDE
    /// </summary>
    internal sealed partial class MainWindow : RibbonWindow
    {
        public MainWindow(ChameleonCoderApp app)
        {
            App = app;

            Model = new ViewModel.MainWindowModel(app);
            ModelClientHelper.InitializeModel(Model);

            Model.ViewChanged -= AdjustView;
            Model.ViewChanged += AdjustView;

            Model.SelectFile -= ModelClientHelper.SelectFile;
            Model.SelectFile += ModelClientHelper.SelectFile;

            DataContext = Model;
            CommandBindings.AddRange(Model.Commands);

            InitializeComponent();
            App.PluginMan.ModuleLoaded += ModuleLoaded;
            App.PluginMan.ModuleUnloaded += ModuleUnloaded;
            App.PluginMan.ServiceExecute += ServiceExecute;
            App.PluginMan.ServiceExecuted += ServiceExecuted;

            ChameleonCoderCommands.OpenNewTab.Execute(null, this);
        }

        internal ChameleonCoderApp App
        {
            get;
            private set;
        }

        private ViewModel.MainWindowModel Model
        {
            get;
            set;
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

                    if (App.ResourceMan.ActiveResource != null)
                        App.ResourceMan.Close();
                    break;

                case CCTabPage.ResourceList:

                    ribbon.ContextualTabSet = ribbon.ContextualTabSets[0];
                    ribbon.SelectedTabItem = ribbon.ContextualTabSet.Tabs[0];

                    if (App.ResourceMan.ActiveResource != null)
                        App.ResourceMan.Close();
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

                    if (App.ResourceMan.ActiveResource != null)
                        App.ResourceMan.Close();

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
            System.Windows.Input.NavigationCommands.Refresh.Execute(null, Model.ActiveTab.Content as IInputElement);
        }

        #region temp
        private void ModuleLoaded(object sender, Plugins.ModuleEventArgs e)
        {
            if (e.Module != null)
            {
                if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
                {
                    ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                        CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        e.Module.Name, e.Module.Version, e.Module.Author, e.Module.About)));
                }
                else
                    CurrentModule.Text = string.Format(Properties.Resources.ModuleInfo,
                        e.Module.Name, e.Module.Version, e.Module.Author, e.Module.About);
            }
        }

        private void ModuleUnloaded(object sender, Plugins.ModuleEventArgs e)
        {
            if (e.Module != null)
            {
                if (!ChameleonCoderApp.RunningApp.Dispatcher.CheckAccess())
                {
                    ChameleonCoderApp.RunningApp.Dispatcher.BeginInvoke(new Action(() =>
                        CurrentModule.Text = string.Empty));
                }
                else
                    CurrentModule.Text = string.Empty;
            }
        }

        private void ServiceExecute(object sender, Plugins.ServiceEventArgs e)
        {
            if (e.Service != null)
            {
                CurrentActionProgress.IsIndeterminate = true;
                CurrentAction.Text = string.Format(Properties.Resources.ServiceInfo, e.Service.Name, e.Service.Version, e.Service.Author, e.Service.About);
            }
        }

        private void ServiceExecuted(object sender, Plugins.ServiceEventArgs e)
        {
            if (e.Service != null)
            {
                CurrentActionProgress.IsIndeterminate = false;
                CurrentAction.Text = string.Empty;
            }
        }
        #endregion

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (IsInitialized)
                ChameleonCoderCommands.SetGroupingMode.Execute((sender as RibbonToggleButton).IsChecked == true,
                    Model.ActiveTab.Content as IInputElement);
        }

        private void SortingChanged(object sender, EventArgs e)
        {
            if (IsInitialized)
                ChameleonCoderCommands.SetSortingMode.Execute((sender as RibbonToggleButton).IsChecked == true,
                    Model.ActiveTab.Content as IInputElement);
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
                            ChameleonCoderCommands.OpenFileManagementPage.Execute(null, this);
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
