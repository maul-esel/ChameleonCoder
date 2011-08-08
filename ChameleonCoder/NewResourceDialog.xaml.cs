using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für NewResourceDialog.xaml
    /// </summary>
    public partial class NewResourceDialog : Window
    {
        #region constructors
        public NewResourceDialog(IResource parent)
        {
            #region templates
            var templates = new List<ITemplate>(PluginManager.GetTemplates());

            foreach (var type in ResourceTypeManager.GetResourceTypes())
                if (!Attribute.IsDefined(type, typeof(NoWrapperTemplateAttribute)))
                    templates.Add(new AutoTemplate(type, ResourceTypeManager.GetInfo(type)));
            #endregion

            #region groups
            List<string> groups = new List<string>();

            foreach (var templ in templates)
            {
                string group = string.IsNullOrWhiteSpace(templ.Group) ? Properties.Resources.PropertyGroup_General : templ.Group;
                if (!groups.Contains(group))
                    groups.Add(group);
            }
            #endregion

            InitializeComponent();

            DataContext = new { Lang = App.Gui.MVVM, groups = groups, templates = templates };
            Owner = App.Gui;
            ParentResource = parent;
        }

        public NewResourceDialog() : this(null) { }
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

            IResource child = template.Create(ParentResource, name);
            if (child != null)
                ResourceManager.Add(child, ParentResource);
        }
        #endregion

        IResource ParentResource;
    }
}
