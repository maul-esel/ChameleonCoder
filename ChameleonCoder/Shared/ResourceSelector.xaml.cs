using System;
using System.Collections.Generic;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Shared
{
    /// <summary>
    /// a dialog to let a user select one or several resources
    /// </summary>
    public sealed partial class ResourceSelector : Window
    {
        #region constructors

        /// <summary>
        /// creates a new instance of the ResourceSelector dialog,
        /// displaying a caller-defined set of resources
        /// and letting the use select a caller-defined number of resources.
        /// </summary>
        internal ResourceSelector(ViewModel.ResourceSelectorModel model)
        {
            model.ActiveResourceNeeded -= GetActiveResource;
            model.ActiveResourceNeeded += GetActiveResource;

            CommandBindings.AddRange(model.Commands);

            DataContext = model;
            InitializeComponent();
        }

        #endregion

        private void NotifyModel(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (DataContext as ViewModel.ResourceSelectorModel).NotifyActiveResourceChanged();
        }

        private void GetActiveResource(object sender, ViewModel.Interaction.InformationEventArgs<IComponent> e)
        {
            e.Information = catalog.SelectedItem;
        }

        private void FinishDialog(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
