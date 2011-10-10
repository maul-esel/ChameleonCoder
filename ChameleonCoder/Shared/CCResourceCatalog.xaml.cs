using System;
using System.Windows;
using System.Windows.Controls;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Shared
{
    /// <summary>
    /// a user control that displays a caller-defined set of resources
    /// </summary>
    public sealed partial class CCResourceCatalog : UserControl
    {
        /// <summary>
        /// creates a new instance of the CCResourceCatalog control which displays all registered resources
        /// </summary>
        public CCResourceCatalog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// the DependencyProperty field for the <see cref="Collection"/> property
        /// </summary>
        public static DependencyProperty CollectionProperty = DependencyProperty.Register("Collection",
                                                                        typeof(ResourceCollection),
                                                                        typeof(CCResourceCatalog));

        /// <summary>
        /// the collection to be shown
        /// </summary>
        public ResourceCollection Collection
        {
            get
            {
                return (ResourceCollection)GetValue(CollectionProperty);
            }
            set
            {
                SetValue(CollectionProperty, value);
                TreeView.DataContext = value;
                value.CollectionChanged += (s, e) => DataContext = s;
            }
        }

        /// <summary>
        /// the DependencyProperty field for the <see cref="ShowReferences"/> property
        /// </summary>
        public static DependencyProperty ShowReferencesProperty = DependencyProperty.Register("ShowReferences",
                                                                            typeof(bool),
                                                                            typeof(CCResourceCatalog));

        /// <summary>
        /// gets or sets whether the control shows references or not
        /// </summary>
        public bool ShowReferences
        {
            get
            {
                return (bool)GetValue(ShowReferencesProperty);
            }
            set
            {
                SetValue(ShowReferencesProperty, value);
                ((Converter.CollectionCombineConverter)TreeView.FindResource("Converter")).IgnoreReferences = !value;
            }
        }

        /// <summary>
        /// a private method to expand or de-expand a treeview node on selection
        /// </summary>
        /// <param name="sender">the control which raises the event</param>
        /// <param name="e">the arguments for this event</param>
        private void Expand(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem != null) // if an item is selected
            {
                TreeViewItem item = Find(TreeView); // find the TreeViewItem for the SelectedItem (which is an IResource instance)
                if (item != null) // if found:
                    item.IsExpanded = !item.IsExpanded; // toggle expand state
            }
        }

        /// <summary>
        /// a private helper method to find the item for the SelectedItem property
        /// </summary>
        /// <param name="parent">the ItemsControl to iterate through</param>
        /// <returns>the TreeViewItem if found, null othwerwise</returns>
        private TreeViewItem Find(ItemsControl parent)
        {
            ItemsControl child;
            // search for the item in parent's Items
            TreeViewItem item = parent.ItemContainerGenerator.ContainerFromItem(TreeView.SelectedItem) as TreeViewItem;

            int i = 0;
            // if the item is not in parent's items:
            while (item == null && i < parent.Items.Count)
            {
                child = parent.ItemContainerGenerator.ContainerFromIndex(i) as ItemsControl; // get the next item
                if (child != null)
                    item = Find(child); // recurse through the item's Children
                i++;
            }

            return item; // return the item or null
        }

        /// <summary>
        /// a wrapper event for the TreeView's SelectedItemChanged event
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Object> SelectedItemChanged
        {
            add
            {
                TreeView.SelectedItemChanged += value;
            }
            remove
            {
                TreeView.SelectedItemChanged -= value;
            }
        }

        /// <summary>
        /// a wrapper property for the TreeView's SelectedItem property
        /// </summary>
        [System.ComponentModel.BindableAttribute(true)]
        public IComponent SelectedItem
        {
            get
            {
                return TreeView.SelectedItem as IComponent;
            }
        }
    }
}
