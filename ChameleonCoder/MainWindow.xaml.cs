using System;
using System.Collections.Generic;
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
        internal MainWindow()
        {
            InitializeComponent();

            if (ServiceHost.GetServiceCount() == 0)
                this.MenuServices.IsEnabled = false;

            foreach (IService service in ServiceHost.GetServices())
                MenuServices.Items.Add(new RibbonApplicationMenuItem() { Image = service.Icon, Header = service.ServiceName, DataContext = service });

            foreach (Type t in ResourceTypeManager.GetResourceTypes())
            {
                Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(t);

                this.visTypes.Items.Add(t);
                this.MenuCreators.Items.Add(new RibbonApplicationMenuItem() { Image = info.TypeIcon, Header = info.DisplayName, DataContext = t });

                RibbonMenuItem item = new RibbonMenuItem(){ Header = info.DisplayName, Image = info.TypeIcon, DataContext = t };
                item.Click += ResourceCreateChild;
                this.AddChildResource.Items.Add(item);
            }

            this.Tabs.Items.Add(new KeyValuePair<string, Page>("welcome", new WelcomePage()));
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
            CollectionViewSource.GetDefaultView((((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as ResourceListPage).ResourceList.ItemsSource).Refresh();
        }

        private void GoHome(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab(typeof(WelcomePage))) != -1)
                Tabs.SelectedIndex = i;
            else
                Tabs.SelectedIndex = Tabs.Items.Add(new KeyValuePair<string, Page>("welcome", new WelcomePage()));
        }

        private void GoList(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab(typeof(ResourceListPage))) != -1)
                Tabs.SelectedIndex = i;
            else
                Tabs.SelectedIndex = Tabs.Items.Add(new KeyValuePair<string, Page>("resource list", new ResourceListPage()));
        }

        private void GoSettings(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab(typeof(SettingsPage))) != -1)
                Tabs.SelectedIndex = i;
            else
                Tabs.SelectedIndex = Tabs.Items.Add(new KeyValuePair<string, Page>("settings", new SettingsPage()));
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (IsInitialized)
                (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as ResourceListPage).GroupingChanged(EnableGroups.IsChecked);
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            RibbonApplicationMenuItem item = e.OriginalSource as RibbonApplicationMenuItem;
            if (item != null)
            {
                IService service = item.DataContext as IService;

                CurrentActionProgress.IsIndeterminate = true;
                CurrentAction.Text = string.Format(Properties.Resources.ServiceInfo, service.ServiceName, service.Version, service.Author, service.About);

                service.Call();
                while (service.IsBusy) ;

                CurrentActionProgress.IsIndeterminate = false;
                CurrentAction.Text = string.Empty;
            }
        }

        #region resources

        private void ResourceCreate(object sender, RoutedEventArgs e)
        {
            Type resourceType = (e.OriginalSource as RibbonApplicationMenuItem).DataContext as Type;
            Resources.ResourceTypeInfo prop = ResourceTypeManager.GetInfo(resourceType);

            if (prop != null)
                prop.Creator(resourceType, null, ResourceManager.Add);
        }

        private void ResourceCreateChild(object sender, RoutedEventArgs e)
        {
            Type resourceType = (e.OriginalSource as RibbonMenuItem).DataContext as Type;
            Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(resourceType);

            if (info != null)
                info.Creator(resourceType, ResourceManager.ActiveItem, ResourceManager.Add);
        }

        private void ResourceDelete(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("are you sure you want to delete this resource?", "CC - deleting resource...", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                    {
                        Tabs.Items.Insert(Tabs.SelectedIndex + 1, new KeyValuePair<string, Page>(resource.Name + " [edit]", new Navigation.EditPage(editResource)));
                        Tabs.SelectedIndex++;
                    }
                    else
                        Tabs.SelectedIndex = i;
                }
            }
        }

        private void ResourceSave(object sender, EventArgs e)
        {
            ResourceSave((KeyValuePair<string, Page>)Tabs.SelectedItem);
        }

        private void ResourceSave(KeyValuePair<string, Page> page)
        {
            ResourceSave(page.Value as EditPage);
        }

        private void ResourceSave(EditPage page)
        {
            if (page != null)
                page.Save();
        }

        private void ResourceOpen(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ResourceOpen(TreeView.SelectedItem as IResource, true);
        }

        private void ResourceOpen(object sender, RoutedPropertyChangedEventArgs<BreadcrumbItem> e)
        {
            if (e.NewValue.IsRoot)
                GoHome(null, null);
            else
                ResourceOpen(ResourceHelper.GetResourceFromPath(breadcrumb.PathFromBreadcrumbItem(e.NewValue)), e.OldValue.IsRoot);
        }

        internal void ResourceOpen(IResource resource, bool createNew)
        {
            if (ResourceManager.ActiveItem != null)
                ResourceManager.ActiveItem.Save();

            if (resource != null)
            {
                ResourceManager.Open(resource);

                int i = FindResourceTab(resource, false);
                if (i == -1)
                {
                    if (createNew || Tabs.SelectedIndex == -1)
                    {
                        Tabs.Items.Insert(Tabs.SelectedIndex + 1, new KeyValuePair<string, Page>(resource.Name, new ResourceViewPage(resource)));
                        Tabs.SelectedIndex++;
                    }
                    else
                    {
                        Tabs.Items.Insert(Tabs.SelectedIndex, new KeyValuePair<string, Page>(resource.Name, new ResourceViewPage(resource)));
                        Tabs.Items.RemoveAt(Tabs.SelectedIndex);
                    }
                }
                else
                    Tabs.SelectedIndex = i;
            }
        }

        private void ResourcesPackage(object sender, EventArgs e)
        {
            CurrentAction.Text = Properties.Resources.PackagerInfo;
            CurrentActionProgress.IsIndeterminate = true;

            Interaction.ResourceSelector selector = new Interaction.ResourceSelector();
            if (selector.ShowDialog() == true)
                App.PackageResources(selector.resources);

            CurrentAction.Text = string.Empty;
            CurrentActionProgress.IsIndeterminate = false;
        }

        #endregion

        private void Restart(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Current.Shutdown(0);
        }

        #region Tabs
        private void TabClosed(object sender, RoutedEventArgs e)
        {
            DependencyObject ctrl = (e.OriginalSource as Button).Parent;

            for (int i = 0; i < 4; i++)
                ctrl = VisualTreeHelper.GetParent(ctrl);

            KeyValuePair<string, Page> item = (KeyValuePair<string, Page>)(ctrl as TabItem).DataContext;
            ResourceSave(item);
            Tabs.Items.Remove(item);

            TabChanged(null, null);
        }

        private void TabChanged(object sender, EventArgs e)
        {
            if (Tabs.SelectedItem == null)
                return;

            Type selected = ((KeyValuePair<string, Page>)Tabs.SelectedItem).Value.GetType();
            breadcrumb.Visibility = Visibility.Visible;

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
                breadcrumb.Visibility = Visibility.Hidden;
            }

            else if (selected == typeof(EditPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[1];
                ResourceManager.ActiveItem = (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as EditPage).Resource;
                breadcrumb.Path = ResourceManager.ActiveItem.GetPath();
            }

            else if (selected == typeof(ResourceViewPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[2];
                ResourceManager.ActiveItem = (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as ResourceViewPage).Resource;
                breadcrumb.Path = ResourceManager.ActiveItem.GetPath();
            }

            else if (selected == typeof(SettingsPage))
            {
                Ribbon.ContextualTabSet = null;
                ResourceManager.ActiveItem = null;
                breadcrumb.Visibility = Visibility.Hidden;
            }
        }

        private int FindResourceTab(IResource resource, bool useEdit)
        {
            ResourceViewPage rvPage;
            EditPage edPage;

            int i = -1;
            foreach (KeyValuePair<string, Page> page in Tabs.Items)
            {
                i++;
                if ((rvPage = page.Value as ResourceViewPage) != null && !useEdit)
                    if (rvPage.Resource == resource)
                        return i;
                if ((edPage = page.Value as EditPage) != null && useEdit)
                    if (edPage.Resource as IResource == resource)
                        return i;
                i++;
            }
            return -1;
        }

        internal int FindPageTab(Type type)
        {
            int i = -1;
            foreach (KeyValuePair<string, Page> page in Tabs.Items)
            {
                i++;
                if (page.Value.GetType() == type)
                    return i;
            }
            return -1;
        }
        #endregion
    }
}
