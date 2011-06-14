using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Windows.Controls.Ribbon;
using ChameleonCoder.Resources.Collections;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Grid2.Visibility = System.Windows.Visibility.Hidden;
            this.Grid3.Visibility = System.Windows.Visibility.Hidden;

            ResourceManager.FlatList = (ResourceCollection)this.Resources["resources"];
            ResourceManager.children = (ResourceCollection)this.Resources["resourceHierarchy"];

            this.TreeView.Items.SortDescriptions.Clear();
            this.TreeView.Items.SortDescriptions.Add(new SortDescription("Type", ListSortDirection.Ascending));
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
            this.Grid1.Visibility = System.Windows.Visibility.Visible;
            this.Grid2.Visibility = System.Windows.Visibility.Hidden;
            this.Grid3.Visibility = System.Windows.Visibility.Hidden;
        }

        private void OpenAResource(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            ResourceBase old = e.OldValue as ResourceBase;
            if (old != null)
                old.Save();

            ResourceBase resource = e.NewValue as ResourceBase;

            if (resource != null)
            {
                resource.Open();
                
                this.Grid1.Visibility = System.Windows.Visibility.Hidden;
                this.Grid2.Visibility = System.Windows.Visibility.Hidden;
                this.Grid3.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(this.listView1.ItemsSource).Refresh();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
