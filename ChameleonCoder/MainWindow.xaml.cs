using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.LanguageModules;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Services;
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

            this.CurrentActionProgress.IsEnabled = false;
            
            foreach (IService service in ServiceHost.GetServices())
                this.MenuServices.Items.Add(service);

            if (ServiceHost.GetServiceCount() == 0)
                this.MenuServices.IsEnabled = false;

            foreach (Type t in ResourceTypeManager.GetResourceTypes())
            {
                this.visTypes.Items.Add(t);
                this.MenuCreators.Items.Add(t);

                RibbonMenuItem item = new RibbonMenuItem();
                item.Click += this.CreateChild;
                item.Header = ResourceTypeManager.GetInfo(t).DisplayName;
                item.ImageSource = ResourceTypeManager.GetInfo(t).TypeIcon;
                this.AddChildResource.Items.Add(item);
            }

            this.Tabs.Items.Add(new KeyValuePair<string, Page>("welcome", new Navigation.WelcomePage()));
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            this.CurrentActionProgress.IsIndeterminate = true;

            IService service = (e.OriginalSource as RibbonApplicationMenuItem).Header as IService;
            service.Call();
            while (service.IsBusy);

            this.CurrentActionProgress.IsIndeterminate = false;
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

        private void GoHome(object sender, EventArgs e)
        {
            bool found = false;
            int i = -1;
            foreach (object o in App.Gui.Tabs.Items)
            {
                i++;
                if (((KeyValuePair<string, Page>)o).Value.GetType() == typeof(Navigation.WelcomePage))
                {
                    found = true;
                    App.Gui.Tabs.SelectedIndex = i;
                    break;
                }
            }
            if (!found)
            {
                int after = App.Gui.Tabs.Items.Add(new KeyValuePair<string, Page>("resources", new Navigation.WelcomePage()));
                App.Gui.Tabs.SelectedIndex = after;
            }
        }

        private void OpenAResource(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            IResource old = e.OldValue as IResource;
            if (old != null)
                old.Save();
            
            IResource resource = e.NewValue as IResource;

            if (resource != null)
            {
                LanguageModuleHost.UnloadModule();

                ResourceManager.ActiveItem = resource;

                ILanguageResource languageResource;
                if ((languageResource = resource as ILanguageResource) != null)
                    LanguageModuleHost.LoadModule(languageResource.language);

                int i = Tabs.Items.Add(new KeyValuePair<string, Page>(resource.Name, new Navigation.ResourceViewPage(resource)));
                Tabs.SelectedIndex = i;
            }
        }

        private void EditAResource(object sender, EventArgs e)
        {
            if (ResourceManager.ActiveItem != null)
            {
                IResource resource = ResourceManager.ActiveItem;
                IResolvable link;

                while ((link = resource as IResolvable) != null && link.shouldResolve)
                    resource = link.Resolve();

                IEditable editResource = resource as IEditable;
                if (editResource != null)
                    Tabs.SelectedItem = new KeyValuePair<string, Page>(resource.Name + " [edit]", new Navigation.EditPage(editResource));
            }
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
                (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as Navigation.ResourceListPage).GroupingChanged(this.EnableGroups.IsChecked);
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            //CollectionViewSource.GetDefaultView(this.ResourceList.ItemsSource).Refresh();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            Application.Current.Shutdown(0);
        }

        private void CreateResource(object sender, RoutedEventArgs e)
        {
            Type resourceType = (e.OriginalSource as RibbonApplicationMenuItem).Header as Type;
            Resources.ResourceTypeInfo prop = ResourceTypeManager.GetInfo(resourceType);

            if (prop != null)
                prop.Creator(resourceType, null, ResourceManager.Add);
        }

        private void CreateChild(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((e.OriginalSource as RibbonMenuItem).Header as Type != null).ToString());

            Type resourceType = (e.OriginalSource as RibbonMenuItem).Header as Type;
            Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(resourceType);

            if (info != null)
                info.Creator(resourceType, ResourceManager.ActiveItem, ResourceManager.Add);
        }

        private void DeleteResource(object sender, RoutedEventArgs e)
        {
            ResourceManager.ActiveItem.Delete();
        }

        private void DroppedFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string file in files)
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    try { doc.Load(file); }
                    catch (System.Xml.XmlException ex)
                    {
                        // check if it is a package file
                        MessageBox.Show(ex.Message + ex.Source);
                    }

                    App.AddResource(doc.DocumentElement, null, null);
                    System.IO.File.Copy(file,
                        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                        + System.IO.Path.DirectorySeparatorChar + "Data" + System.IO.Path.DirectorySeparatorChar
                        + System.IO.Path.GetFileName(file));
                }
            }
        }

        private void CloseTab(object sender, EventArgs e)
        {
        }

        private void TabChanged(object sender, EventArgs e)
        {
            Type selected = ((KeyValuePair<string, Page>)Tabs.SelectedItem).Value.GetType();

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
                ResourceManager.ActiveItem = (((KeyValuePair<string, Page>)Tabs.SelectedItem).Value as Navigation.EditPage).Resource;
            }
            else
                this.contextEditing.Visibility = Visibility.Collapsed;
        }
    }
}
