using System;
using System.Collections.Generic;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// Interaktionslogik für ResourceSelector.xaml
    /// </summary>
    public partial class ResourceSelector : Window
    {
        #region constructors
        public ResourceSelector() : this(ResourceManager.GetChildren(), -1)
        {            
        }

        public ResourceSelector(int maxResources)
            : this(ResourceManager.GetChildren(), -1)
        {
            maxCount = maxResources;
        }

        public ResourceSelector(Resources.ResourceCollection resources)
            : this(resources, -1)
        {
        }

        public ResourceSelector(Resources.ResourceCollection resources, int maxResources)
        {
            InitializeComponent();
            Catalog.Collection = resources;
            Catalog.SelectedItemChanged += ValidateButtons;
            DataContext = new { Lang = App.Gui.DataContext };

            OKButton.Click += (sender, e) =>
            {
                DialogResult = true;
                Close();
            };
        }
        #endregion

        int maxCount = -1;

        public List<IResource> resources = new List<IResource>();

        private void AddResource(object sender, EventArgs e)
        {
            if (Catalog.SelectedItem == null)
                return;

            if (resources.Count < maxCount || maxCount == -1)
            {
                resources.Add(Catalog.SelectedItem as IResource);
                ValidateButtons(null, null);
            }
        }

        private void RemoveResource(object sender, EventArgs e)
        {
            if (Catalog.SelectedItem == null)
                return;

            resources.Remove(Catalog.SelectedItem as IResource);
            ValidateButtons(null, null);
        }

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
            DataContext = new { Lang = App.Gui.DataContext, Res = Catalog.SelectedItem };
        }
    }
}
