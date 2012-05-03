using System;
using System.Windows.Data;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.UI.Navigation
{
    /// <summary>
    /// a Page displaying all registered plugins
    /// </summary>
    internal sealed partial class PluginPage : System.Windows.Controls.Page
    {
        /// <summary>
        /// creates a new instance of the page
        /// </summary>
        public PluginPage(ViewModel.PluginPageModel model)
        {
            ModelClientHelper.InitializeModel(model);

            model.PluginNeeded -= ModelClientHelper.SelectFile; // remove handler if already attached
            model.PluginNeeded += ModelClientHelper.SelectFile; // add handler

            model.RepresentationNeeded -= ModelClientHelper.GetModelRepresentation; // see above
            model.RepresentationNeeded += ModelClientHelper.GetModelRepresentation;

            DataContext = model;
            CommandBindings.AddRange(model.Commands);

            InitializeComponent();            
        }

        /// <summary>
        /// updates the filter
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private void RefreshFilter(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();
        }

        /// <summary>
        /// filters the listboxitems to fit the type selection
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private void Filter(object sender, FilterEventArgs e)
        {
            if (IsInitialized && categories.SelectedIndex != -1)
            {
                int category = categories.SelectedIndex;

                if (category == 0) // if all plugins should be shown:
                    e.Accepted = true; // accept everything
                else
                {
                    if (e.Item is ITemplate) // compare selected category with required one
                        e.Accepted = category == 1;
                    if (e.Item is IService)
                        e.Accepted = category == 2;
                    if (e.Item is ILanguageModule)
                        e.Accepted = category == 3;
                    if (e.Item is IResourceFactory)
                        e.Accepted = category == 4;
                    if (e.Item is IRichContentFactory)
                        e.Accepted = category == 5;
                }
            }
        }
    }
}
