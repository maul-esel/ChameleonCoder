using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für ResourceListPage.xaml
    /// </summary>
    public partial class ResourceListPage : Page
    {
        internal ResourceListPage()
        {
            this.Resources.Add("Name_", Properties.Resources.Name_Name);
            this.Resources.Add("Description_",Properties.Resources.Name_Description);
            InitializeComponent();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            IResource resource = e.Item as IResource;
            e.Accepted = true;
            Type resType = resource.GetType();

            if (resource is IResolvable && !App.Gui.ShowLinks.IsChecked == true)
                e.Accepted = false;

            else if (IsInitialized)
            {
                int i = 0;
                foreach (Type t in App.Gui.visTypes.Items)
                {
                    if (t == resType)
                    {
                        DependencyObject item = App.Gui.visTypes.ItemContainerGenerator.ContainerFromIndex(i);
                        if (item != null)
                        {
                            for (int index = 0; index < 14; index++)
                                item = VisualTreeHelper.GetChild(item, index == 6 ? 1 : index == 3 || index == 8 || index == 11 ? 3 : 0); /* /0/0/0/3/0/0/1/0/3/0/0/3/0/0 */
                            if ((item as CheckBox).IsChecked == true)
                                e.Accepted = false;
                        }
                        break;
                    }
                    i++;
                }   
            }
        }

        private void OpenResource(object sender, EventArgs e)
        {
            App.Gui.ResourceOpen(ResourceList.SelectedItem as IResource);
        }

        private void DroppedFile(object sender, DragEventArgs e)
        {
            App.Gui.DroppedFile(sender, e);
        }

        internal void GroupingChanged(bool? enabled)
        {
            if (this.IsInitialized)
            {
                if (enabled == true)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Add(new PropertyGroupDescription(null, new Converter.CustomGroupConverter()));
                else if (enabled == false)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Clear();
            }
        }
    }
}
