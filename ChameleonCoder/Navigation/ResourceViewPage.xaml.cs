using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying resource details
    /// </summary>
    internal sealed partial class ResourceViewPage : Page
    {
        /// <summary>
        /// creates a new instance of this page, given a resource to display
        /// </summary>
        /// <param name="resource">the resource to display</param>
        internal ResourceViewPage(IResource resource)
        {
            ResourceManager.Open(Resource = resource);
            InitializeComponent();
            DataContext = new ViewModel.ResourceViewPageModel(Resource);
        }

        internal void AddMetadata(string name)
        {
            Resource.SetMetadata(name, null);
            Update();
        }

        internal void DeleteMetadata()
        {
            if (MetadataGrid.SelectedIndex != -1)
            {
                Resource.DeleteMetadata(((KeyValuePair<string, string>)MetadataGrid.SelectedItem).Key);
                Update();
            }
        }

        private void SaveMetadata(object sender, EventArgs e)
        {
        	var box = sender as TextBox;
            string value = box.Text;
            string key = ((KeyValuePair<string, string>)((box.TemplatedParent as ContentPresenter).Parent as GridViewRowPresenter).Content).Key;

            Resource.SetMetadata(key, value);
            box.InvalidateProperty(System.Windows.FrameworkElement.WidthProperty);
        }

        private void Update()
        {
            (DataContext as ViewModel.ResourceViewPageModel).UpdateAll();
        }

        /// <summary>
        /// the resource which is displayed
        /// </summary>
        internal IResource Resource { get; private set; }
    }
}
