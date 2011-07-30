using System;
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

            this.Tabs.Items.Add(new TabContext("Home", new WelcomePage()));
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
            int i;
            if ((i = FindPageTab<WelcomePage>()) != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext("Home", new WelcomePage()), Tabs.SelectedIndex);
        }

        internal void GoList(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab<ResourceListPage>()) != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext("resource list", new ResourceListPage()), Tabs.SelectedIndex);
        }

        internal void GoSettings(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab<SettingsPage>()) != -1)
                Tabs.SelectedIndex = i;
            else
                TabReplace(new TabContext("settings", new SettingsPage()), Tabs.SelectedIndex);
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
                        Tabs.Items.Insert(Tabs.SelectedIndex + 1, new TabContext(resource.Name + " [edit]", new Navigation.EditPage(editResource)));
                        Tabs.SelectedIndex++;
                    }
                    else
                        Tabs.SelectedIndex = i;
                }
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
                {
                    Tabs.Items.Insert(Tabs.SelectedIndex, new TabContext(resource.Name, new ResourceViewPage(resource)));
                    Tabs.Items.RemoveAt(Tabs.SelectedIndex);
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
        private void TabOpen(object sender, EventArgs e)
        {
            Tabs.SelectedIndex = Tabs.Items.Add(new TabContext("Home", new WelcomePage()));
        }

        private void TabClosed(object sender, RoutedEventArgs e)
        {
            DependencyObject ctrl = (e.OriginalSource as Button).Parent;

            for (int i = 0; i < 4; i++)
                ctrl = VisualTreeHelper.GetParent(ctrl);

            TabContext item = (ctrl as TabItem).DataContext as TabContext;
            ResourceSave(item);
            Tabs.Items.Remove(item);

            TabChanged(null, null);
        }

        private void TabChanged(object sender, EventArgs e)
        {
            if (Tabs.Items.Count == 0)
            {
                Tabs.Items.Add(new TabContext("Home", new WelcomePage()));
                return;
            }
            if (Tabs.SelectedItem == null)
                return;

            Type selected = (Tabs.SelectedItem as TabContext).Content.GetType();

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
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/Resources";
            }

            else if (selected == typeof(EditPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[1];
                ResourceManager.ActiveItem = ((Tabs.SelectedItem as TabContext).Content as EditPage).Resource;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/Resources" + ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString);
            }

            else if (selected == typeof(ResourceViewPage))
            {
                Ribbon.ContextualTabSet = Ribbon.ContextualTabSets[2];
                ResourceManager.ActiveItem = ((Tabs.SelectedItem as TabContext).Content as ResourceViewPage).Resource;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/Resources" + ResourceManager.ActiveItem.GetPath(breadcrumb.SeparatorString);
            }

            else if (selected == typeof(SettingsPage))
            {
                Ribbon.ContextualTabSet = null;
                ResourceManager.ActiveItem = null;
                breadcrumb.Path = breadcrumb.PathFromBreadcrumbItem(breadcrumb.RootItem) + "/Settings";
            }
        }

        private void TabReplace(TabContext newTab, int oldTab)
        {
            Tabs.Items.Insert(oldTab + 1, newTab);
            Tabs.Items.RemoveAt(oldTab);
            Tabs.SelectedIndex = oldTab;
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
