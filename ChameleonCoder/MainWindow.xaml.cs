using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Navigation;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
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
            DataContext = MVVM.Instance;
            CommandBindings.AddRange(MVVM.Instance.Commands);
            InitializeComponent();
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            // TODO: replace by command (see next line)
            //System.Windows.Input.NavigationCommands.Refresh.Execute(null, MVVM.Instance.ActiveTab.Content as Page);
            CollectionViewSource.GetDefaultView((MVVM.Instance.ActiveTab.Content as ResourceListPage).ResourceList.ItemsSource).Refresh();
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (IsInitialized)
                (MVVM.Instance.ActiveTab.Content as ResourceListPage).GroupingChanged((sender as RibbonToggleButton).IsChecked == true);
        }

        [Obsolete("to be moved to model", false)]
        private void GoHome()
        {
            System.Windows.Input.NavigationCommands.BrowseHome.Execute(null, this);
        }

        [Obsolete("to be moved to model", false)]
        internal void GoList()
        {
            ChameleonCoderCommands.OpenResourceListPage.Execute(null, this);
        }

        [Obsolete("to be moved to model", false)]
        internal void GoPlugins()
        {
            ChameleonCoderCommands.OpenPluginPage.Execute(null, this);
        }

        [Obsolete("to be moved to model", false)]
        internal void GoSettings()
        {
            ChameleonCoderCommands.OpenSettingsPage.Execute(null, this);
        }        

        #region resources

        private void ResourceCopy(object sender, EventArgs e)
        {
            Interaction.ResourceSelector selector = new Interaction.ResourceSelector(1);
            if (selector.ShowDialog() == true
                && selector.ResourceList.Count > 0
                && selector.ResourceList[0] != ResourceManager.ActiveItem)
            {
                ResourceManager.ActiveItem.Copy(selector.ResourceList[0]);
            }
        }

        private void ResourceMove(object sender, EventArgs e)
        {
            Interaction.ResourceSelector selector = new Interaction.ResourceSelector(1);
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

        [Obsolete("to be moved to model + renamed", false)]
        private void ResourceOpen(object sender, RoutedPropertyChangedEventArgs<BreadcrumbItem> e)
        {
            if (e.NewValue != null)
            {
                BreadcrumbContext context = e.NewValue.DataContext as BreadcrumbContext;

                if (context != null)
                {
                    if (context.PageType == Interaction.CCTabPage.Home)
                        GoHome();
                    else if (context.PageType == Interaction.CCTabPage.ResourceList)
                        GoList();
                    else if (context.PageType == Interaction.CCTabPage.Settings)
                        GoSettings();
                    else if (context.PageType == Interaction.CCTabPage.Plugins)
                        GoPlugins();
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

        private void MetadataAdd(object sender, EventArgs e)
        {
            var input = new Interaction.InputBox(Properties.Resources.Status_CreateMeta, Properties.Resources.Meta_EnterName);
            if (input.ShowDialog() == true && !string.IsNullOrWhiteSpace(input.Text))
                (MVVM.Instance.ActiveTab.Content as ResourceViewPage).AddMetadata(input.Text);
        }

        private void MetadataDelete(object sender, EventArgs e)
        {
            (MVVM.Instance.ActiveTab.Content as ResourceViewPage).DeleteMetadata();
        }

        private void SortingChanged(object sender, EventArgs e)
        {
            if (IsInitialized)
                (MVVM.Instance.ActiveTab.Content as ResourceListPage).SortingChanged((sender as RibbonToggleButton).IsChecked == true);
        }

        #region Tabs
        [Obsolete("to be moved to model", false)]
        private void TabChanged(object sender, EventArgs e)
        {
            if (MVVM.Instance.Tabs.Count == 0)
            {
                GoHome();
            }
        }
        #endregion

        // to be moved to edit page model + use commands!
        #region editing methods

        [Obsolete("to be moved to edit page model", false)]
        private void EditSearch(object sender, EventArgs e)
        {
            EditPerformAction(editor =>
                {
                    var dialog = new CCSearchReplaceDialog(
                            () => editor.Text,
                            (offset, length, replaceBy) => editor.Document.Replace(offset, length, replaceBy),
                            (offset, length) =>
                            {
                                editor.Select(offset, length);
                                var loc = editor.Document.GetLocation(offset);
                                editor.ScrollTo(loc.Line, loc.Column);
                            },
                            false);
                    dialog.ShowDialog();
                });
        }

        [Obsolete("to be moved to edit page model", false)]
        private void EditReplace(object sender, EventArgs e)
        {
            EditPerformAction(editor =>
                {
                    var dialog = new CCSearchReplaceDialog(
                            () => editor.Text,
                            (offset, length, replaceBy) => editor.Document.Replace(offset, length, replaceBy),
                            (offset, length) =>
                            {
                                editor.Select(offset, length);
                                var loc = editor.Document.GetLocation(offset);
                                editor.ScrollTo(loc.Line, loc.Column);
                            },
                            true);
                    dialog.ShowDialog();
                });
        }

        private void EditPerformAction(Action<ICSharpCode.AvalonEdit.TextEditor> action)
        {
            EditPage edit = MVVM.Instance.ActiveTab.Content as EditPage;
            if (edit != null)
                action(edit.Editor);
        }

        #endregion
    }
}
