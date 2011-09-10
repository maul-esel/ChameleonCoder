using System;
using System.Collections.Generic;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// a dialog to let a user select a resource
    /// </summary>
    public partial class ResourceSelector : Window
    {
        #region constructors
        /// <summary>
        /// creates a new instance of the ResourceSelector dialog,
        /// displaying all registered resources
        /// and letting the user select an unlimited number of resources.
        /// </summary>
        public ResourceSelector() : this(ResourceManager.GetChildren(), -1)
        {            
        }

        /// <summary>
        /// creates a new instance of the ResourceSelector dialog,
        /// displaying all registered resources
        /// and letting the use select a caller-defined number of resources.
        /// </summary>
        /// <param name="maxResources">the number of resources the user can select</param>
        public ResourceSelector(int maxResources)
            : this(ResourceManager.GetChildren(), maxResources)
        {
        }

        /// <summary>
        /// creates a new instance of the ResourceSelector dialog,
        /// displaying a caller-defined set of resources
        /// and letting the user select an unlimited number of resources.
        /// </summary>
        /// <param name="resources">the set of resources the user can select from</param>
        public ResourceSelector(Resources.ResourceCollection resources)
            : this(resources, -1)
        {
        }

        /// <summary>
        /// creates a new instance of the ResourceSelector dialog,
        /// displaying a caller-defined set of resources
        /// and letting the use select a caller-defined number of resources.
        /// </summary>
        /// <param name="resources">the set of resources the user can select from</param>
        /// <param name="maxResources">the number of resources the user can select</param>
        public ResourceSelector(Resources.ResourceCollection resources, int maxResources)
        {
            InitializeComponent();
            Catalog.Collection = resources;
            Catalog.SelectedItemChanged += ValidateButtons;
            DataContext = new { Lang = App.Gui.DataContext };
            maxCount = maxResources;

            OKButton.Click += (sender, e) =>
            {
                DialogResult = true;
                Close();
            };
        }
        #endregion

        int maxCount = -1;

        public List<IResource> resources = new List<IResource>();

        /// <summary>
        /// adds a resource to the list of selected resources
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void AddResource(object sender, EventArgs e)
        {
            if (Catalog.SelectedItem == null)
                return;

            resources.Add(Catalog.SelectedItem as IResource);
            ValidateButtons(null, null);
        }

        /// <summary>
        /// removes a resource from the list of selected resources
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void RemoveResource(object sender, EventArgs e)
        {
            if (Catalog.SelectedItem == null)
                return;

            resources.Remove(Catalog.SelectedItem as IResource);
            ValidateButtons(null, null);
        }

        /// <summary>
        /// checks the add- and remove-button and adjusts their visibility
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void ValidateButtons(object sender, EventArgs e)
        {
            if (Catalog.SelectedItem == null)
                return;
            
            if (resources.Contains(Catalog.SelectedItem as IResource))
            {
                AddButton.Visibility = Visibility.Hidden;
                RemButton.Visibility = Visibility.Visible;
            }
            else
            {
                AddButton.Visibility = Visibility.Visible;
                RemButton.Visibility = Visibility.Hidden;
            }

            if (resources.Count >= maxCount)
                AddButton.Visibility = Visibility.Hidden;

            DataContext = new { Lang = App.Gui.DataContext, Res = Catalog.SelectedItem };
        }
    }
}
