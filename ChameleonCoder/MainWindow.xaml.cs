using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Base;
using ChameleonCoder.Resources.Collections;
using ChameleonCoder.LanguageModules;
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

            this.Editor.Visibility = System.Windows.Visibility.Hidden;

            this.PropertyGrid.Visibility = System.Windows.Visibility.Hidden;
            this.MetadataGrid.Visibility = System.Windows.Visibility.Hidden;
            this.NotesBox.Visibility = System.Windows.Visibility.Hidden;

            ResourceManager.FlatList = (ResourceCollection)this.Resources["resources"];
            ResourceManager.children = (ResourceCollection)this.Resources["resourceHierarchy"];

            this.TreeView.Items.SortDescriptions.Clear();
            this.TreeView.Items.SortDescriptions.Add(new SortDescription("Type", ListSortDirection.Ascending));
            
            foreach (IService service in ServiceHost.GetServices())
            {
                RibbonApplicationMenuItem item = new RibbonApplicationMenuItem();
                item.Header = service.ServiceName; item.ImageSource = service.Icon;
                item.Click += LaunchService;
                this.MenuServices.Items.Add(item);
            }

            if (ServiceHost.GetServiceCount() == 0)
            {
                this.MenuServices.IsEnabled = false;
            }
            
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            ServiceHost.CallService(new Guid());
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            ResourceBase resource = e.Item as ResourceBase;
            e.Accepted = true;

            switch (resource.Type)
            {
                case ResourceType.link:
                    if (this.ShowLinks.IsChecked == false)
                        e.Accepted = false;
                    break;
                case ResourceType.file:
                    if (this.HideFiles.IsChecked == true)
                        e.Accepted = false;
                    break;
                case ResourceType.code:
                    if (this.HideCodes.IsChecked == true)
                        e.Accepted = false;
                    break;
                case ResourceType.library:
                    if (this.HideLibraries.IsChecked == true)
                        e.Accepted = false;
                    break;
                case ResourceType.project:
                    if (this.HideProjects.IsChecked == true)
                        e.Accepted = false;
                    break;
                case ResourceType.task:
                    if (this.HideTasks.IsChecked == true)
                        e.Accepted = false;
                    break;
            }
        }

        private void GoHome(object sender, EventArgs e)
        {
            ResourceManager.ActiveItem = null;
            LanguageModuleHost.UnloadModule();

            this.ResourceList.Visibility = System.Windows.Visibility.Visible;

            this.Editor.Visibility = System.Windows.Visibility.Hidden;

            this.PropertyGrid.Visibility = System.Windows.Visibility.Hidden;
            this.MetadataGrid.Visibility = System.Windows.Visibility.Hidden;
            this.NotesBox.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OpenAResource(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            ResourceBase old = e.OldValue as ResourceBase;
            if (old != null)
                old.Save();

            ResourceBase resource = e.NewValue as ResourceBase;

            if (resource != null)
            {
                LanguageModuleHost.UnloadModule();

                //resource.Open();

                ResourceManager.ActiveItem = resource;

                if (resource is CodeResource)
                    LanguageModuleHost.LoadModule((resource as CodeResource).language);
                else if (resource is ProjectResource)
                    LanguageModuleHost.LoadModule((resource as ProjectResource).language);
                
                this.ResourceList.Visibility = System.Windows.Visibility.Hidden;

                this.Editor.Visibility = System.Windows.Visibility.Hidden;

                this.PropertyGrid.Visibility = System.Windows.Visibility.Visible;
                this.MetadataGrid.Visibility = System.Windows.Visibility.Visible;
                this.NotesBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void EditAResource(object sender, EventArgs e)
        {
            if (ResourceManager.ActiveItem != null)
            {
                FileResource resource = null;

                if (ResourceManager.ActiveItem is FileResource)
                    resource = (FileResource)ResourceManager.ActiveItem;
                else if (ResourceManager.ActiveItem is LinkResource && ((LinkResource)ResourceManager.ActiveItem).Resolve() is FileResource)
                    resource = (FileResource)((LinkResource)ResourceManager.ActiveItem).Resolve();

                if (resource != null)
                {
                    if (!string.IsNullOrWhiteSpace(resource.Path) && System.IO.File.Exists(resource.Path))
                    {
                        this.Editor.Load(resource.Path);

                        this.ResourceList.Visibility = System.Windows.Visibility.Hidden;

                        this.Editor.Visibility = System.Windows.Visibility.Visible;

                        this.PropertyGrid.Visibility = System.Windows.Visibility.Hidden;
                        this.MetadataGrid.Visibility = System.Windows.Visibility.Hidden;
                        this.NotesBox.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (this.EnableGroups.IsChecked == false)
            {
                CollectionViewSource.GetDefaultView(this.ResourceList.ItemsSource).GroupDescriptions.Clear();
            }
            else if (this.EnableGroups.IsChecked == true)
            {
                if (this.ResourceList != null)
                {
                    object source = this.ResourceList.ItemsSource;
                    if (source != null)
                        CollectionViewSource.GetDefaultView(source).GroupDescriptions.Add(new PropertyGroupDescription("Type"));
                }
            }

        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(this.ResourceList.ItemsSource).Refresh();
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
            ResourceCreator creator = new ResourceCreator();
        }
    }
}
