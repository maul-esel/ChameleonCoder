using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying all registered resources
    /// </summary>
    public sealed partial class ResourceListPage : Page
    {
        /// <summary>
        /// creates a new instance of this page
        /// </summary>
        internal ResourceListPage()
        {
            DataContext = App.Gui.DataContext;
            InitializeComponent();
        }

        /// <summary>
        /// filters the items in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Filter(object sender, FilterEventArgs e)
        {
            IResource resource = e.Item as IResource; // get resource
            e.Accepted = true; // default value: accept
            Type resType = resource.GetType(); // get type

            // if IResolvables should not be shown and this is an IResolvable:
            if (resource is IResolvable && !App.Gui.ShowLinks.IsChecked == true)
                e.Accepted = false; // directly filter it

            else if (IsInitialized) // othwerise, and if the page is initialized:
            {
                int i = 0;
                foreach (Type t in App.Gui.visTypes.Items) // iterate through types
                {
                    if (t == resType) // if it is a match:
                    {
                        DependencyObject item = App.Gui.visTypes.ItemContainerGenerator.ContainerFromIndex(i); // get the item containing this type
                        if (item != null)
                        {
                            for (int index = 0; index < 14; index++)
                                item = VisualTreeHelper.GetChild(item, index == 6 ? 1 : index == 3 || index == 8 || index == 11 ? 3 : 0); // recurse through the visual tree /* /0/0/0/3/0/0/1/0/3/0/0/3/0/0 */
                            if ((item as CheckBox).IsChecked == true) // until we find the checkbox. If it is checked, hide the resource
                                e.Accepted = false;
                        }
                        break; // stop iteration
                    }
                    i++;
                }   
            }
        }

        /// <summary>
        /// opens a resource in ResourceView
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void OpenResource(object sender, EventArgs e)
        {
            App.Gui.ResourceOpen(ResourceList.SelectedItem as IResource); // redirect call
        }

        /// <summary>
        /// updates the grouping status
        /// </summary>
        /// <param name="enabled">true to enable grouping, otherwise false</param>
        internal void GroupingChanged(bool enabled)
        {
            if (IsInitialized) // if the page is initialized
            {
                if (enabled) // if enabled: add group description (group by CustomGroupConverter)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Add(new PropertyGroupDescription(null, new Converter.CustomGroupConverter(), StringComparison.CurrentCulture));
                else // othwerwise: remove group descriptions
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Clear();
            }
        }

        /// <summary>
        /// updates sorting status
        /// </summary>
        /// <param name="enabled">true to enable sorting, otherwise false</param>
        internal void SortingChanged(bool enabled)
        {
            if (IsInitialized) // if the page is initialized
            {
                if (enabled) // if enabled: add SortDescription (sort by name, ascending)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                else // otherwise: remove sort descriptions
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).SortDescriptions.Clear();
            }
        }
    }
}
