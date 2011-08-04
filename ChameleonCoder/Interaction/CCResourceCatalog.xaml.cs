using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// Interaktionslogik für CCResourceCatalog.xaml
    /// </summary>
    public partial class CCResourceCatalog : UserControl
    {
        #region constructors
        public CCResourceCatalog(ResourceCollection top)
        {
            Init(top);
        }        

        public CCResourceCatalog(IResource parent) : this(parent, true) { }

        public CCResourceCatalog(IResource parent, bool includeSelf)
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "the parent resource must not be null!");

            ResourceCollection top;

            if (!includeSelf)
                top = parent.children;
            else
            {
                top = new ResourceCollection();
                top.Add(parent);
            }

            Init(top);
        }

        public CCResourceCatalog() : this(ResourceManager.GetChildren()) { }

        private void Init(ResourceCollection top)
        {
            InitializeComponent();
            Collection = top;
        }

        #endregion

        public ResourceCollection Collection
        {
            set
            {
                TreeView.DataContext = value;
            }
        }

        #region OnResourceDropped
        bool isImporting = false;

        public bool ImportDropped
        {
            set
            {
                if (value && !isImporting)
                {
                    Drop += ImportDroppedResource;
                    isImporting = true;
                }
                else if (!value && isImporting)
                {
                    Drop -= ImportDroppedResource;
                    isImporting = false;
                }
            }
            get
            {
                return isImporting;
            }
        }

        public new event DragEventHandler Drop
        {
            add
            {
                TreeView.Drop += value;
            }
            remove
            {
                TreeView.Drop -= value;
            }
        }

        private void ImportDroppedResource(object sender, DragEventArgs e)
        {
            App.ImportDroppedResource(e);
        }
        #endregion

        private void Expand(object sender, RoutedEventArgs e)
        {
            if (TreeView.SelectedItem != null)
            {
                TreeViewItem item = Find(TreeView);
                if (item != null)
                    item.IsExpanded = !item.IsExpanded;
            }
        }

        private TreeViewItem Find(ItemsControl parent)
        {
            ItemsControl child;
            TreeViewItem item = parent.ItemContainerGenerator.ContainerFromItem(TreeView.SelectedItem) as TreeViewItem;

            int i = 0;
            while (item == null && i < parent.Items.Count)
            {
                child = parent.ItemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                if (child != null)
                    item = Find(child);
                i++;
            }

            return item;
        }

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

        public new event MouseButtonEventHandler MouseDoubleClick
        {
            add
            {
                TreeView.MouseDoubleClick += value;
            }
            remove
            {
                TreeView.MouseDoubleClick -= value;
            }
        }

        [System.ComponentModel.BindableAttribute(true, System.ComponentModel.BindingDirection.OneWay)]
        public IResource SelectedItem
        {
            get
            {
                return TreeView.SelectedItem as IResource;
            }
        }
    }
}
