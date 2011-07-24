using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für CCResourceCatalog.xaml
    /// </summary>
    public partial class CCResourceCatalog : UserControl
    {
        public CCResourceCatalog(Resources.ResourceCollection top)
        {
            this.Init(top);
        }

        public CCResourceCatalog(Resources.Interfaces.IResource parent) : this(parent, true) { }

        public CCResourceCatalog(Resources.Interfaces.IResource parent, bool includeSelf)
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "the parent resource must not be null!");

            Resources.ResourceCollection top;
            if (!includeSelf)
                top = parent.children;
            else
            {
                top = new Resources.ResourceCollection();
                top.Add(parent);
            }
            this.Init(top);
        }

        public CCResourceCatalog()
        {
            this.Init(ChameleonCoder.Resources.Management.ResourceManager.GetChildren());
        }

        protected void Init(Resources.ResourceCollection top)
        {
            InitializeComponent();
            this.TreeView.DataContext = top;
        }

        bool isImporting;

        public bool ImportDropped
        {
            set
            {
                if (value && !isImporting)
                {
                    this.Drop += this.ImportDroppedResource;
                    isImporting = true;
                }
                else if (!value && isImporting)
                {
                    this.Drop -= this.ImportDroppedResource;
                    isImporting = false;
                }
            }
        }

        private void ImportDroppedResource(object sender, DragEventArgs e)
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

        public new event DragEventHandler Drop
        {
            add
            {
                this.TreeView.Drop += value;
            }
            remove
            {
                this.TreeView.Drop -= value;
            }
        }

        public event RoutedPropertyChangedEventHandler<Object> SelectedItemChanged
        {
            add
            {
                this.TreeView.SelectedItemChanged += value;
            }
            remove
            {
                this.TreeView.SelectedItemChanged -= value;
            }
        }

        public new event MouseButtonEventHandler MouseDoubleClick
        {
            add
            {
                this.TreeView.MouseDoubleClick += value;
            }
            remove
            {
                this.TreeView.MouseDoubleClick -= value;
            }
        }

        [System.ComponentModel.BindableAttribute(true)]
        public Object SelectedItem
        {
            get
            {
                return this.TreeView.SelectedItem;
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
