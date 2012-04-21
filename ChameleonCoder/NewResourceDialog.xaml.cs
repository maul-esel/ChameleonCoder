using System;
using System.Windows;
using System.Windows.Data;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder
{
    /// <summary>
    /// the dialog to let the user create a new resource
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    internal sealed partial class NewResourceDialog : Window
    {
        #region constructors

        internal NewResourceDialog(ChameleonCoderApp app, IResource parent)
        {
            DataContext = new ViewModel.NewResourceDialogModel(this.app = app);
            InitializeComponent();

            Owner = app.Gui;
            ParentResource = parent;
        }

        #endregion

        #region methods

        private void Filter(object sender, FilterEventArgs e)
        {
            string selectedGroup = groupList.SelectedItem as string;
            string templateGroup = (e.Item as ITemplate).Group;
            e.Accepted = templateGroup == selectedGroup
                || (string.IsNullOrWhiteSpace(templateGroup) && selectedGroup == Properties.Resources.PropertyGroup_General);
        }

        private void RefreshFilter(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(list.ItemsSource).Refresh();
        }

        private void FinishDialog(object sender, EventArgs e)
        {
            ITemplate template = list.SelectedItem as ITemplate;
            string name = newName.Text;

            DialogResult = true;
            Close();

            IResource child = template.Create(ParentResource, name, app.DefaultFile);
            if (child != null)
                app.ResourceMan.Add(child, ParentResource);
        }

        #endregion

        IResource ParentResource;
        private readonly ChameleonCoderApp app = null;
    }
}
