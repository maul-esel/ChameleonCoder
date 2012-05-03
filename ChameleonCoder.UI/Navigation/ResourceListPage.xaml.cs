using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using ChameleonCoder.Resources;

namespace ChameleonCoder.UI.Navigation
{
    /// <summary>
    /// a page displaying all registered resources
    /// </summary>
    internal sealed partial class ResourceListPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// creates a new instance of this page
        /// </summary>
        internal ResourceListPage(ViewModel.ResourceListPageModel model)
        {
            ModelClientHelper.InitializeModel(model);
            DataContext = this.model = model;

            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh,
                RefreshList));
            CommandBindings.Add(new CommandBinding(ChameleonCoderCommands.SetSortingMode,
                SortingChanged));
            CommandBindings.Add(new CommandBinding(ChameleonCoderCommands.SetGroupingMode,
                GroupingChanged));
            CommandBindings.AddRange(model.Commands);

            InitializeComponent();

            (FindResource("colorConv") as Converters.AppConverterBase).App = model.App;
            (FindResource("resourceTypeConv") as Converters.AppConverterBase).App = model.App;
            (FindResource("modIconConv") as Converters.AppConverterBase).App = model.App;
        }

        private readonly ViewModel.ResourceListPageModel model = null;

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
            
            if (IsInitialized) // if the page is initialized:
            {
                int i = 0;
                // TODO: loop once, and store result?
                // HACK: do not access type list like so!
                /*
                 * MAYBE: 
                 *  - a dictionary holding all types + bools
                 *  - assume all types are enabled
                 *  - disable or re-enable a type via command 
                 */
                foreach (Type t in ((MainWindow)((ChameleonCoderApp)model.App).Window).visTypes.Items) // iterate through types
                {
                    if (t == resType) // if it is a match:
                    {
                        DependencyObject item = ((MainWindow)((ChameleonCoderApp)model.App).Window).visTypes.ItemContainerGenerator.ContainerFromIndex(i); // get the item containing this type
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
        private void OpenResource(object sender, RoutedEventArgs e)
        {
            ChameleonCoderCommands.OpenResourceView.Execute((e.OriginalSource as FrameworkElement).DataContext,
                this);
        }

        /// <summary>
        /// changes the sorting mode based on the command parameter passed
        /// </summary>
        /// <param name="sender">the object raising the event</param>
        /// <param name="e">additional data related to the event</param>
        private void SortingChanged(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsInitialized)
            {
                if ((bool)e.Parameter)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
                else // otherwise: remove sort descriptions
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).SortDescriptions.Clear();
            }
        }

        /// <summary>
        /// changes the grouping mode based on the command parameter passed
        /// </summary>
        /// <param name="sender">the object raising the event</param>
        /// <param name="e">additional data related to the event</param>
        private void GroupingChanged(object sender, ExecutedRoutedEventArgs e)
        {
            if (IsInitialized) // if the page is initialized
            {
                if ((bool)e.Parameter) // if enabled: add group description (group by CustomGroupConverter)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Add(new PropertyGroupDescription(null, this.Resources["groupConv"] as Converters.CustomGroupConverter, StringComparison.CurrentCulture));
                else // othwerwise: remove group descriptions
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Clear();
            }
        }

        /// <summary>
        /// refreshes the list containing all resources
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshList(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).Refresh();
        }
    }
}
