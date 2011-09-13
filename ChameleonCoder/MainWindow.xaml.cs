﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Navigation;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using Odyssey.Controls;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal sealed partial class MainWindow : RibbonWindow
    {
        internal ViewModel MVVM { get { return DataContext as ViewModel; } }

        internal MainWindow()
        {
            InitializeComponent();

            if (PluginManager.ServiceCount == 0)
                this.MenuServices.IsEnabled = false;

            foreach (IService service in PluginManager.GetServices())
                MenuServices.Items.Add(new RibbonApplicationMenuItem() { Image = service.Icon, Header = service.Name, DataContext = service.Identifier });

            foreach (Type t in ResourceTypeManager.GetResourceTypes())
                visTypes.Items.Add(t);

            GoHome(null, null);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown(0);
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(((Tabs.SelectedItem as TabContext).Content as ResourceListPage).ResourceList.ItemsSource).Refresh();
        }

        private void GoHome(object sender, EventArgs e)
        {
            int i = FindPageTab<WelcomePage>();
            if (i != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext(ViewModel.Item_Home, new WelcomePage()), Tabs.SelectedIndex);
        }

        internal void GoList(object sender, EventArgs e)
        {
            int i = FindPageTab<ResourceListPage>();
            if (i != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext(ViewModel.Item_List, new ResourceListPage()), Tabs.SelectedIndex);
        }

        internal void GoPlugins(object sender, EventArgs e)
        {
            int i = FindPageTab<PluginPage>();
            if (i != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext(ViewModel.Item_Plugins, new PluginPage()), Tabs.SelectedIndex);
        }

        internal void GoSettings(object sender, EventArgs e)
        {
            int i = FindPageTab<SettingsPage>();
            if (i != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext(ViewModel.Item_Settings, new SettingsPage()), Tabs.SelectedIndex);
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
                && selector.resourceList.Count > 0
                && selector.resourceList[0] != ResourceManager.ActiveItem)
            {
                ResourceManager.ActiveItem.Copy(selector.resourceList[0]);
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
                IResource resource = ResourceManager.ActiveItem;
                IResolvable link;

                while ((link = resource as IResolvable) != null && link.ShouldResolve)
                    resource = link.Resolve();

                IEditable editResource = resource as IEditable;
                if (editResource != null)
                {
                    int i = FindResourceTab(resource, true);
                    if (i == -1)
                        TabReplace(new TabContext(string.Format(Properties.Resources.Item_ResourceEdit, resource.Name), new Navigation.EditPage(editResource)), Tabs.SelectedIndex);
                    else
                        Tabs.SelectedIndex = i;
                }
            }
        }

        private void ResourceMove(object sender, EventArgs e)
        {
            Interaction.ResourceSelector selector = new Interaction.ResourceSelector(1);
            if (selector.ShowDialog() == true // user did not cancel
                && selector.resourceList.Count > 0 // user selected 1 resource
                && selector.resourceList[0] != null // resource is not null
                && selector.resourceList[0] != ResourceManager.ActiveItem.Parent) // resource is already parent
            {
                if (!selector.resourceList[0].IsDescendantOf(ResourceManager.ActiveItem)) // can't be moved to descendant
                {
                    ResourceManager.ActiveItem.Move(selector.resourceList[0]);
                }
                else
                    MessageBox.Show(string.Format(Properties.Resources.Error_MoveToDescendant, ResourceManager.ActiveItem.Name, selector.resourceList[0].Name),
                                    Properties.Resources.Status_Move);
            }
        }

        private void ResourceOpen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ResourceOpen(TreeView.SelectedItem as IResource);
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

                int i = FindResourceTab(resource, false);
                if (i == -1)
                    TabReplace(new TabContext(string.Format(Properties.Resources.Item_ResourceView, resource.Name), new ResourceViewPage(resource)), Tabs.SelectedIndex);
                else
                    Tabs.SelectedIndex = i;
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
            MVVM.Tabs.Add(new TabContext(Properties.Resources.Item_Home, new WelcomePage()));
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

        private void TabChanged(object sender, EventArgs e)
        {
            if (Tabs.Items.Count == 0)
            {
                GoHome(null, null);
                return;
            }
            if (Tabs.SelectedItem == null)
                return;

            Page page = (Tabs.SelectedItem as TabContext).Content;
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
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_List;
            }

            else if (page is EditPage)
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[1];
                ResourceManager.Open(((Tabs.SelectedItem as TabContext).Content as EditPage).Resource);
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_List + ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString);
            }

            else if (page is ResourceViewPage)
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[2];
                ResourceManager.Open(((Tabs.SelectedItem as TabContext).Content as ResourceViewPage).Resource);
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_List + ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString);
            }

            else if (page is SettingsPage)
            {
                Ribbon.ContextualTabSet = null;
                if (ResourceManager.ActiveItem != null)
                    ResourceManager.Close();
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_Settings;
            }
            Ribbon.SelectedTabIndex = Ribbon.Tabs.Count;
        }

        private void TabReplace(TabContext newTab, int oldTab)
        {
            if (oldTab >= 0 && oldTab < MVVM.Tabs.Count && MVVM.Tabs.Count != 0)
                MVVM.Tabs[oldTab] = newTab;
            else
                MVVM.Tabs.Add(newTab);
        }

        private int FindResourceTab(IResource resource, bool useEdit)
        {
            ResourceViewPage rvPage;
            EditPage edPage;

            int i = -1;
            foreach (TabContext page in Tabs.Items)
            {
                i++;
                if ((rvPage = page.Content as ResourceViewPage) != null && !useEdit)
                    if (rvPage.Resource == resource)
                        return i;
                if ((edPage = page.Content as EditPage) != null && useEdit)
                    if (edPage.Resource as IResource == resource)
                        return i;
                i++;
            }
            return -1;
        }

        internal int FindPageTab<T>() where T : Page
        {
            int i = -1;
            foreach (TabContext page in Tabs.Items)
            {
                i++;
                if (page.Content.GetType() == typeof(T))
                    return i;
            }
            return -1;
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
            // todo
        }

        private void EditReplace(object sender, EventArgs e)
        {
            // todo
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
