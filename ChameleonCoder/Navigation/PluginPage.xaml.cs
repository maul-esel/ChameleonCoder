using System;
using System.Windows.Data;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a Page displaying all registered plugins
    /// </summary>
    internal sealed partial class PluginPage : CCPageBase
    {
        /// <summary>
        /// creates a new instance of the page
        /// </summary>
        public PluginPage()
        {
            ViewModel.PluginPageModel.Instance.PluginNeeded -= FindAssembly; // remove handler if already attached
            ViewModel.PluginPageModel.Instance.PluginNeeded += FindAssembly; // add handler

            ViewModel.PluginPageModel.Instance.RepresentationNeeded -= OpenDialog; // see above
            ViewModel.PluginPageModel.Instance.RepresentationNeeded += OpenDialog;

            Initialize(ViewModel.PluginPageModel.Instance);
            InitializeComponent();            
        }

        /// <summary>
        /// lets the user select an assembly for the model
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private static void FindAssembly(object sender, ViewModel.Interaction.FileSelectionEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog() { Filter = "plugins | *.dll",
                                                                            Title = e.Message,
                                                                            CheckPathExists = e.MustExist,
                                                                            InitialDirectory = e.Directory})
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    e.Path = dialog.FileName;
                }
            }
        }

        /// <summary>
        /// creates a PluginInstaller dialog for the model
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        /// <remarks>This must not be moved to the model.</remarks>
        private static void OpenDialog(object sender, ViewModel.Interaction.RepresentationEventArgs e)
        {
            var model = e.Model as ViewModel.PluginInstallerModel;
            if (model != null)
            {
                e.Representation = new PluginInstaller(model);
            }
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
                }
            }
        }
    }
}
