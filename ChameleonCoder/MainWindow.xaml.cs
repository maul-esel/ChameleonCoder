﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Navigation;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Services;
using Odyssey.Controls;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        internal ViewModel MVVM { get { return DataContext as ViewModel; } }

        internal MainWindow()
        {
            InitializeComponent();

            if (ComponentManager.ServiceCount == 0)
                this.MenuServices.IsEnabled = false;

            foreach (IService service in ComponentManager.GetServices())
                MenuServices.Items.Add(new RibbonApplicationMenuItem() { Image = service.Icon, Header = service.Name, DataContext = service.Identifier });

            foreach (Type t in ResourceTypeManager.GetResourceTypes())
            {
                Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(t);

                visTypes.Items.Add(t);
                MenuCreators.Items.Add(new RibbonApplicationMenuItem() { Image = info.TypeIcon, Header = info.DisplayName, DataContext = t });

                RibbonMenuItem item = new RibbonMenuItem(){ Header = info.DisplayName, Image = info.TypeIcon, DataContext = t };
                item.Click += ResourceCreateChild;
                AddChildResource.Items.Add(item);
            }
            foreach (var template in ComponentManager.GetTemplates())
                MenuCreators.Items.Add(new RibbonApplicationMenuItem() { Image = template.Icon, Header = template.Name, DataContext = template.Identifier });

            GoHome(null, null);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown(0);
        }

        internal void DroppedFile(object sender, DragEventArgs e)
        {
            App.ImportDroppedResource(e);
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
                TabReplace(new TabContext(MVVM.Item_Home, new WelcomePage()), Tabs.SelectedIndex);
        }

        internal void GoList(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab<ResourceListPage>()) != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext(MVVM.Item_List, new ResourceListPage()), Tabs.SelectedIndex);
        }

        internal void GoSettings(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab<SettingsPage>()) != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext(MVVM.Item_Settings, new SettingsPage()), Tabs.SelectedIndex);
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (IsInitialized)
                ((Tabs.SelectedItem as TabContext).Content as ResourceListPage).GroupingChanged(EnableGroups.IsChecked);
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            RibbonApplicationMenuItem item = e.OriginalSource as RibbonApplicationMenuItem;
            if (item != null)
                ComponentManager.CallService((Guid)item.DataContext);
        }

        #region resources

        private void ResourceCreate(object sender, RoutedEventArgs e)
        {
            Type resourceType = (e.OriginalSource as RibbonApplicationMenuItem).DataContext as Type;
            if (resourceType != null)
            {
                Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(resourceType);

                if (info != null && info.Create != null)
                {
                    IResource resource = info.Create(resourceType, null);
                    if (resource != null)
                        ResourceManager.Add(resource, null);
                }
            }
            else
            {
                var template = (Guid)(e.OriginalSource as RibbonApplicationMenuItem).DataContext;
                if (template != default(Guid))
                    ComponentManager.Create(template, null);
            }
        }

        private void ResourceCreateChild(object sender, RoutedEventArgs e)
        {
            Type resourceType = (e.OriginalSource as RibbonMenuItem).DataContext as Type;
            Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(resourceType);

            if (info != null && info.Create != null)
            {
                IResource resource = info.Create(resourceType, ResourceManager.ActiveItem);
                if (resource != null)
                    ResourceManager.Add(resource, ResourceManager.ActiveItem);
            }
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

                while ((link = resource as IResolvable) != null && link.shouldResolve)
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
            if (selector.ShowDialog() == true)
            {
                ResourceManager.ActiveItem.Move(selector.resources[0]);
            }
        }

        private void ResourceOpen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ResourceOpen(TreeView.SelectedItem as IResource);
        }

        private void ResourceOpen(object sender, RoutedPropertyChangedEventArgs<BreadcrumbItem> e)
        {
            BreadcrumbContext context = e.NewValue.DataContext as BreadcrumbContext;
            if (e.NewValue.IsRoot)
                GoHome(null, null);
            else if (context != null && context.IsResourceList)
                GoList(null, null);
            else if (context != null && context.IsSettingsPage)
                GoSettings(null, null);
            else
                ResourceOpen(ResourceHelper.GetResourceFromPath(breadcrumb.PathFromBreadcrumbItem(e.NewValue), breadcrumb.SeparatorString));
        }

        internal void ResourceOpen(IResource resource)
        {
            if (ResourceManager.ActiveItem != null)
                ResourceManager.ActiveItem.Save();

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

        private void ResourcesPackage(object sender, EventArgs e)
        {
            CurrentAction.Text = Properties.Resources.Status_Pack;
            CurrentActionProgress.IsIndeterminate = true;

            Interaction.ResourceSelector selector = new Interaction.ResourceSelector();
            if (selector.ShowDialog() == true && selector.resources.Count > 0)
            {
                BackgroundWorker worker = new BackgroundWorker() { WorkerSupportsCancellation = false };
                worker.DoWork += (bw, args) => PackageManager.PackageResources(selector.resources);
                worker.RunWorkerCompleted += (bw, args) =>
                {
                    if (args.Error != null)
                        MessageBox.Show(Properties.Resources.Error_Package + "\n\n" + args.Error.ToString());
                    else
                        MessageBox.Show(Properties.Resources.Pack_Finished);

                    CurrentAction.Text = string.Empty;
                    CurrentActionProgress.IsIndeterminate = false;
                };
                worker.RunWorkerAsync();
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

        private void ResourcesUnpackage(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "CC Packages (*.ccp)|*.ccp";
            dialog.InitialDirectory = Interaction.InformationProvider.ProgrammingDirectory;
            dialog.FileOk += (s, args) =>
                {
                    if (!args.Cancel)
                        PackageManager.UnpackResources((s as System.Windows.Forms.OpenFileDialog).FileName);
                };

            dialog.ShowDialog();
        }

        #endregion

        private void Restart(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Current.Shutdown(0);
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

            Type selected = (Tabs.SelectedItem as TabContext).Content.GetType();
            Ribbon.SelectedTabIndex = 0;

            if (selected == typeof(WelcomePage))
            {
                Ribbon.ContextualTabSet = null;
                ResourceManager.ActiveItem = null;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem);
            }

            else if (selected == typeof(ResourceListPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[0];
                ResourceManager.ActiveItem = null;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_List;
            }

            else if (selected == typeof(EditPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[1];
                ResourceManager.ActiveItem = ((Tabs.SelectedItem as TabContext).Content as EditPage).Resource;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_List + ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString);
            }

            else if (selected == typeof(ResourceViewPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[2];
                ResourceManager.ActiveItem = ((Tabs.SelectedItem as TabContext).Content as ResourceViewPage).Resource;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/" + Properties.Resources.Item_List + ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString);
            }

            else if (selected == typeof(SettingsPage))
            {
                Ribbon.ContextualTabSet = null;
                ResourceManager.ActiveItem = null;
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
    }
}
