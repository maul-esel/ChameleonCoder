using System;
using System.Windows;
using System.Windows.Data;
using ChameleonCoder.LanguageModules;
using ChameleonCoder.Resources.Collections;
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

            this.Editor.Visibility = System.Windows.Visibility.Hidden;
            this.CurrentActionProgress.IsEnabled = false;

            this.PropertyGrid.Visibility = System.Windows.Visibility.Hidden;
            this.MetadataGrid.Visibility = System.Windows.Visibility.Hidden;
            this.NotesBox.Visibility = System.Windows.Visibility.Hidden;

            ResourceManager.FlatList = (ResourceCollection)this.Resources["resources"];
            ResourceManager.children = (ResourceCollection)this.Resources["resourceHierarchy"];
            
            foreach (IService service in ServiceHost.GetServices())
                this.MenuServices.Items.Add(service);

            if (ServiceHost.GetServiceCount() == 0)
                this.MenuServices.IsEnabled = false;

            foreach (Type t in ResourceTypeManager.GetResourceTypes())
            {
                this.visTypes.Items.Add(t);
                this.MenuCreators.Items.Add(t);
                this.AddChildResource.Items.Add(t);
            }
        }

        private void LaunchService(object sender, RoutedEventArgs e)
        {
            this.CurrentActionProgress.IsEnabled = true;
            ServiceHost.CallService(new Guid());
            
            // sleep while (service.IsBusy)
            this.CurrentActionProgress.IsEnabled = false;
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
                IResource resource = ResourceManager.ActiveItem;
                IResolvable link;

                while ((link = resource as IResolvable) != null && link.shouldResolve)
                    resource = link.Resolve();

                IEditable editResource = resource as IEditable;

                if (editResource != null)
                {
                    this.Editor.Text = editResource.GetText();

                    this.ResourceList.Visibility = System.Windows.Visibility.Hidden;

                    this.Editor.Visibility = System.Windows.Visibility.Visible;

                    this.PropertyGrid.Visibility = System.Windows.Visibility.Hidden;
                    this.MetadataGrid.Visibility = System.Windows.Visibility.Hidden;
                    this.NotesBox.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void GroupsChanged(object sender, RoutedEventArgs e)
        {
            if (this.EnableGroups.IsChecked == false)
                CollectionViewSource.GetDefaultView(this.ResourceList.ItemsSource).GroupDescriptions.Clear();

            else if (this.EnableGroups.IsChecked == true && this.IsInitialized)
                CollectionViewSource.GetDefaultView(this.ResourceList.ItemsSource).GroupDescriptions.Add(new PropertyGroupDescription(null, new Converter.CustomGroupConverter()));
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
            Type resourceType = (e.OriginalSource as RibbonApplicationMenuItem).Header as Type;
            Resources.ResourceTypeInfo prop = ResourceTypeManager.GetInfo(resourceType);

            if (prop != null)
                prop.Creator(resourceType, ResourceManager.Add);
        }

        private void CreateChild(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("...");
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

                    App.AddResource(doc.DocumentElement, null);
                    System.IO.File.Copy(file,
                        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                        + System.IO.Path.DirectorySeparatorChar + "Data" + System.IO.Path.DirectorySeparatorChar
                        + System.IO.Path.GetFileName(file));
                }
            }
        }
    }
}
