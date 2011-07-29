using System;
using System.Collections.Generic;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// Interaktionslogik für ResourceSelector.xaml
    /// </summary>
    public partial class ResourceSelector : Window
    {
        int maxCount = -1;

        public List<IResource> resources = new List<IResource>();

        public ResourceSelector()
        {
            InitializeComponent();
            Catalog.SelectedItemChanged += ValidateButtons;
        }

        public ResourceSelector(int maxResources)
            : this()
        {
            maxCount = maxResources;
        }

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
            DataContext = Catalog.SelectedItem;
        }

        private void FinishDialog(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
