using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Navigation;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using Odyssey.Controls;
using ChameleonCoder.Interaction;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal sealed partial class MainWindow : RibbonWindow
    {
        internal ViewModel.MainWindowModel MVVM { get { return DataContext as ViewModel.MainWindowModel; } }

        internal MainWindow()
        {
            InitializeComponent();

            if (PluginManager.ServiceCount == 0)
                this.MenuServices.IsEnabled = false;

            foreach (IService service in PluginManager.GetServices())
                MenuServices.Items.Add(new RibbonApplicationMenuItem() { Image = service.Icon, Header = service.Name, DataContext = service.Identifier });

            foreach (Type t in ResourceTypeManager.GetResourceTypes())
                visTypes.Items.Add(t);

            GoHome();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown(0);
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(((Tabs.SelectedItem as TabContext).Content as ResourceListPage).ResourceList.ItemsSource).Refresh();
        }

        private void GoHome()
        {
            if (Tabs.SelectedItem != null)
            {
                var context = Tabs.SelectedItem as TabContext;
                context.Object = null;
                context.Type = CCTabPage.Home;
                context.Content = new WelcomePage();

                TabChanged(context);
            }
            else
            {
                MVVM.Tabs.Add(new TabContext(CCTabPage.Home, new WelcomePage()));
            }
        }

        private void GoHome(object sender, EventArgs e)
        {
            GoHome();
        }

        internal void GoList()
        {
            var context = Tabs.SelectedItem as TabContext;
            context.Object = null;
            context.Type = CCTabPage.ResourceList;
            context.Content = new ResourceListPage();

            TabChanged(context);
        }

        private void GoList(object sender, EventArgs e)
        {
            GoList();
        }

        internal void GoPlugins()
        {
            var context = Tabs.SelectedItem as TabContext;
            context.Object = null;
            context.Type = CCTabPage.Plugins;
            context.Content = new PluginPage();

            TabChanged(context);
        }

        private void GoPlugins(object sender, EventArgs e)
        {
            GoPlugins();
        }

        internal void GoSettings()
        {
            var context = Tabs.SelectedItem as TabContext;
            context.Object = null;
            context.Type = CCTabPage.Settings;
            context.Content = new SettingsPage();

            TabChanged(context);
        }

        private void GoSettings(object sender, EventArgs e)
        {
            GoSettings();
        }

        private void GroupsChanged(object sender, EventArgs e)
        {
            if (IsInitialized)
                ((Tabs.SelectedItem as TabContext).Content as ResourceListPage).GroupingChanged(EnableGroups.IsChecked == true);
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            RibbonApplicationMenuItem item = e.OriginalSource as RibbonApplicationMenuItem;
            if (item != null)
                PluginManager.CallService((Guid)item.DataContext);
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

        private void ResourceCreate(object sender, RoutedEventArgs e)
        {
            NewResourceDialog dialog = new NewResourceDialog();
            dialog.ShowDialog();
        }

        private void ResourceCreateChild(object sender, RoutedEventArgs e)
        {
            NewResourceDialog dialog = new NewResourceDialog(ResourceManager.ActiveItem);
            dialog.ShowDialog();
        }

        private void ResourceDelete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(string.Format(Properties.Resources.Del_Confirm, ResourceManager.ActiveItem.Name), Properties.Resources.Status_DeleteResource, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                ResourceManager.ActiveItem.Delete();
        }

        private void ResourceEdit(object sender, EventArgs e)
        {
            if (ResourceManager.ActiveItem != null)
            {
                IEditable editResource = ResourceManager.ActiveItem as IEditable;
                if (editResource != null)
                {
                    var context = Tabs.SelectedItem as TabContext;
                    context.Object = editResource.Name;
                    context.Type = CCTabPage.ResourceEdit;
                    context.Content = new EditPage(editResource);

                    TabChanged(context);
                }
            }
        }

        private void ResourceMove(object sender, EventArgs e)
        {
            Interaction.ResourceSelector selector = new Interaction.ResourceSelector(1);
            if (selector.ShowDialog() == true // user did not cancel
                && selector.ResourceList.Count > 0 // user selected 1 resource
                && selector.ResourceList[0] != null // resource is not null
                && selector.ResourceList[0] != ResourceManager.ActiveItem.Parent) // resource is already parent
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

        private void ResourceOpen(object sender, RoutedPropertyChangedEventArgs<BreadcrumbItem> e)
        {
            if (e.NewValue != null)
            {
                BreadcrumbContext context = e.NewValue.DataContext as BreadcrumbContext;

                if (context != null)
                {
                    if (context.Type == BreadcrumbContext.ContextType.Home)
                        GoHome(null, null);
                    else if (context.Type == BreadcrumbContext.ContextType.ResourceList)
                        GoList(null, null);
                    else if (context.Type == BreadcrumbContext.ContextType.Settings)
                        GoSettings(null, null);
                    else if (context.Type == BreadcrumbContext.ContextType.Plugins)
                        GoPlugins(null, null);
                }
                else
                    ResourceOpen(ResourceHelper.GetResourceFromPath(breadcrumb.PathFromBreadcrumbItem(e.NewValue), breadcrumb.SeparatorString));
            }
        }

        internal void ResourceOpen(IResource resource)
        {
            if (resource != null)
            {
                ResourceManager.Open(resource);

                Ribbon.SelectedTabIndex = 0;

                var context = Tabs.SelectedItem as TabContext;
                context.Object = resource.Name;
                context.Type = CCTabPage.ResourceView;
                context.Content = new ResourceViewPage(resource);

                TabChanged(context);
            }
        }

        private void ResourceSave(object sender, EventArgs e)
        {
            ResourceSave(Tabs.SelectedItem as TabContext);
        }

        private void ResourceSave(TabContext page)
        {
            ResourceSave(page.Content as EditPage);
        }

        private void ResourceSave(EditPage page)
        {
            if (page != null)
                page.Save();
        }

        #endregion

        private void MetadataAdd(object sender, EventArgs e)
        {
            var input = new Interaction.InputBox(Properties.Resources.Status_CreateMeta, Properties.Resources.Meta_EnterName);
            if (input.ShowDialog() == true && !string.IsNullOrWhiteSpace(input.Text))
                ((Tabs.SelectedItem as TabContext).Content as ResourceViewPage).AddMetadata(input.Text);
        }

        private void MetadataDelete(object sender, EventArgs e)
        {
            ((Tabs.SelectedItem as TabContext).Content as ResourceViewPage).DeleteMetadata();
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Current.Shutdown(0);
        }

        private void SortingChanged(object sender, EventArgs e)
        {
            if (IsInitialized)
                ((Tabs.SelectedItem as TabContext).Content as ResourceListPage).SortingChanged(SortItems.IsChecked == true);
        }

        #region Tabs
        private void TabOpen(object sender, EventArgs e)
        {
            MVVM.Tabs.Add(new TabContext(CCTabPage.Home, new WelcomePage()));
            Tabs.SelectedIndex = MVVM.Tabs.Count - 1;
        }

        private void TabClosed(object sender, RoutedEventArgs e)
        {
            DependencyObject ctrl = (e.OriginalSource as Button).Parent;

            for (int i = 0; i < 4; i++)
                ctrl = VisualTreeHelper.GetParent(ctrl);

            TabContext item = (ctrl as TabItem).DataContext as TabContext;
            ResourceSave(item);
            MVVM.Tabs.Remove(item);

            TabChanged(null, null);
        }

        private void TabChanged(TabContext newTab)
        {
            Page page = newTab.Content;
            Ribbon.SelectedTabIndex = 0;

            if (page is WelcomePage)
            {
                Ribbon.ContextualTabSet = null;
                if (ResourceManager.ActiveItem != null)
                    ResourceManager.Close();

                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem);
            }

            else if (page is ResourceListPage)
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[0];
                if (ResourceManager.ActiveItem != null)
                    ResourceManager.Close();

                breadcrumb.Path = string.Format("{1}{0}{2}",
                    breadcrumb.SeparatorString,
                    breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem),
                    Properties.Resources.Item_List);
            }

            else if (page is EditPage)
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[1];
                ResourceManager.Open((page as EditPage).Resource);

                breadcrumb.Path = string.Format("{1}{0}{2}{0}{3}",
                    breadcrumb.SeparatorString,
                    breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem),
                    Properties.Resources.Item_List,
                    ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString));
            }

            else if (page is ResourceViewPage)
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[2];
                ResourceManager.Open((page as ResourceViewPage).Resource);

                breadcrumb.Path = string.Format("{1}{0}{2}{0}{3}",
                    breadcrumb.SeparatorString,
                    breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem),
                    Properties.Resources.Item_List,
                    ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString));
            }

            else if (page is SettingsPage)
            {
                Ribbon.ContextualTabSet = null;
                if (ResourceManager.ActiveItem != null)
                    ResourceManager.Close();

                breadcrumb.Path = string.Format("{1}{0}{2}",
                    breadcrumb.SeparatorString,
                    breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem),
                    Properties.Resources.Item_Settings);
            }

            else if (page is PluginPage)
            {
                Ribbon.ContextualTabSet = null;
                if (ResourceManager.ActiveItem != null)
                    ResourceManager.Close();

                breadcrumb.Path = string.Format("{1}{0}{2}",
                    breadcrumb.SeparatorString,
                    breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem),
                    Properties.Resources.Item_Plugins);
            }

            Ribbon.SelectedTabIndex = Ribbon.Tabs.Count;
        }

        private void TabChanged(object sender, EventArgs e)
        {
            if (Tabs.Items.Count == 0)
            {
                GoHome();
                if (Tabs.SelectedItem != null)
                    TabChanged(Tabs.SelectedItem as TabContext);
            }
        }
        #endregion

        #region editing methods

        private void EditPaste(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.Paste());
        }

        private void EditCut(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.Cut());
        }

        private void EditCopy(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.Copy());
        }

        private void EditZoomIn(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.FontSize += 2);            
        }

        private void EditZoomOut(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.FontSize -= 2);
        }

        private void EditUndo(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.Undo());
        }

        private void EditRedo(object sender, EventArgs e)
        {
            EditPerformAction(editor => editor.Redo());
        }

        private void EditSearch(object sender, EventArgs e)
        {
            EditPage edit = (Tabs.SelectedItem as TabContext).Content as EditPage;
            if (edit != null)
            {
                var dialog = new CCSearchReplaceDialog(
                    () => edit.Editor.Text,
                    (offset, length, replaceBy) => edit.Editor.Document.Replace(offset, length, replaceBy),
                    (offset, length) =>
                        {
                            edit.Editor.Select(offset, length);
                            var loc = edit.Editor.Document.GetLocation(offset);
                            edit.Editor.ScrollTo(loc.Line, loc.Column);
                        },
                    false);
                dialog.ShowDialog();
            }
        }

        private void EditReplace(object sender, EventArgs e)
        {
            EditPage edit = (Tabs.SelectedItem as TabContext).Content as EditPage;
            if (edit != null)
            {
                var dialog = new CCSearchReplaceDialog(
                    () => edit.Editor.Text,
                    (offset, length, replaceBy) => edit.Editor.Document.Replace(offset, length, replaceBy),
                    (offset, length) =>
                    {
                        edit.Editor.Select(offset, length);
                        var loc = edit.Editor.Document.GetLocation(offset);
                        edit.Editor.ScrollTo(loc.Line, loc.Column);
                    },
                    true);
                dialog.ShowDialog();
            }
        }

        private void EditPerformAction(Action<ICSharpCode.AvalonEdit.TextEditor> action)
        {
            EditPage edit = (Tabs.SelectedItem as TabContext).Content as EditPage;
            if (edit != null)
                action(edit.Editor);
        }

        #endregion
    }
}
