using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Services;
using ChameleonCoder.Navigation;
using Microsoft.Windows.Controls.Ribbon;

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
                this.MenuServices.Items.Add(service);
            /*foreach (var item in ResourceManager.GetChildren())
                this.breadcrumb.Items.Add(item);*/
            foreach (Type t in ResourceTypeManager.GetResourceTypes())
            {
                this.visTypes.Items.Add(t);
                this.MenuCreators.Items.Add(t);

                RibbonMenuItem item = new RibbonMenuItem();
                item.Click += this.ResourceCreateChild;
                item.Header = ResourceTypeManager.GetInfo(t).DisplayName;
                item.ImageSource = ResourceTypeManager.GetInfo(t).TypeIcon;
                this.AddChildResource.Items.Add(item);
            }

            this.Tabs.Items.Add(new KeyValuePair<string, Page>("welcome", new WelcomePage()));
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown(0);
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            IResource resource = e.Item as IResource;
            e.Accepted = true;

            if (resource is IResolvable && !this.ShowLinks.IsChecked == true)
            {
                e.Accepted = false;
            }
            else
            {
                // check if corresponding type is checked in ShowTypesList
            }
        }

        internal void DroppedFile(object sender, DragEventArgs e)
        {
            App.ImportDroppedResource(e);
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            //CollectionViewSource.GetDefaultView(this.ResourceList.ItemsSource).Refresh();
        }

        private void GoHome(object sender, EventArgs e)
        {
            int i;
            if ((i = FindPageTab(typeof(WelcomePage))) != -1)
                Tabs.SelectedIndex = i;
            else
                Tabs.SelectedIndex = Tabs.Items.Add(new KeyValuePair<string, Page>("welcome", new WelcomePage()));
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
                (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as Navigation.ResourceListPage).GroupingChanged(this.EnableGroups.IsChecked);
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            this.CurrentActionProgress.IsIndeterminate = true;

            IService service;
            RibbonApplicationMenuItem item = e.OriginalSource as RibbonApplicationMenuItem;

            if (item != null)
                (service = item.Header as IService).Call();
            else
                (service = MenuServices.Items[0] as IService).Call();

            while (service.IsBusy) ;

            this.CurrentActionProgress.IsIndeterminate = false;
        }

        #region resources

        private void ResourceCreate(object sender, RoutedEventArgs e)
        {
            Type resourceType = (e.OriginalSource as RibbonApplicationMenuItem).Header as Type;
            Resources.ResourceTypeInfo prop = ResourceTypeManager.GetInfo(resourceType);

            if (prop != null)
                prop.Creator(resourceType, null, ResourceManager.Add);
        }

        private void ResourceCreateChild(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((e.OriginalSource as RibbonMenuItem).Header as Type != null).ToString());

            Type resourceType = (e.OriginalSource as RibbonMenuItem).Header as Type;
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
            ResourceOpen(TreeView.SelectedItem as IResource);
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
                    Tabs.Items.Insert(Tabs.SelectedIndex + 1, new KeyValuePair<string, Page>(resource.Name, new Navigation.ResourceViewPage(resource)));
                    Tabs.SelectedIndex++;
                }
                else
                    Tabs.SelectedIndex = i;
            }
        }

        #endregion

        private void Restart(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Current.Shutdown(0);
        }

        private void TabClosed(object sender, RoutedEventArgs e)
        {
            ResourceSave((KeyValuePair<string, Page>)Tabs.SelectedItem);
            Tabs.Items.Remove((KeyValuePair<string, Page>)(
                System.Windows.Media.VisualTreeHelper.GetParent(
                System.Windows.Media.VisualTreeHelper.GetParent(
                System.Windows.Media.VisualTreeHelper.GetParent(
                System.Windows.Media.VisualTreeHelper.GetParent((e.OriginalSource as Button).Parent as StackPanel)))) as TabItem).DataContext);
            TabChanged(null, null);
        }

        private void TabChanged(object sender, EventArgs e)
        {
            Type selected;
            try { selected = ((KeyValuePair<string, Page>)Tabs.SelectedItem).Value.GetType(); }
            catch (NullReferenceException) { selected = null; }

            if (selected == typeof(Navigation.ResourceListPage))
                this.contextResourceList.Visibility = Visibility.Visible;
            else
                this.contextResourceList.Visibility = Visibility.Collapsed;

            if (selected == typeof(Navigation.ResourceViewPage))
            {
                this.contextResourceView.Visibility = Visibility.Visible;
                ResourceManager.ActiveItem = (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as Navigation.ResourceViewPage).Resource;
            }
            else
                this.contextResourceView.Visibility = Visibility.Collapsed;

            if (selected == typeof(Navigation.EditPage))
            {
                this.contextEditing.Visibility = Visibility.Visible;
                ResourceManager.ActiveItem = (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as EditPage).Resource;
            }
            else
                this.contextEditing.Visibility = Visibility.Collapsed;
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
    }
}
