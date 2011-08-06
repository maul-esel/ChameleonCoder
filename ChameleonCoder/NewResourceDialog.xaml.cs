using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using ChameleonCoder.Templates;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für NewResourceDialog.xaml
    /// </summary>
    public partial class NewResourceDialog : Window
    {
        public NewResourceDialog()
        {
            var templates = new List<ITemplate>(ComponentManager.GetTemplates());

            foreach (var type in ResourceTypeManager.GetResourceTypes())
                templates.Add(new AutoTemplate(type, ResourceTypeManager.GetInfo(type)));

            InitializeComponent();

            (Resources["ListSource"] as CollectionViewSource).Source = templates;
            Owner = App.Gui;
        }
    }
}
