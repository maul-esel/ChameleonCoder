using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Plugins;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für NewResourceDialog.xaml
    /// </summary>
    public partial class NewResourceDialog : Window
    {
        public NewResourceDialog()
        {
            var templates = new List<ITemplate>(PluginManager.GetTemplates());

            foreach (var type in ResourceTypeManager.GetResourceTypes())
                if (!Attribute.IsDefined(type, typeof(NoWrapperTemplateAttribute)))
                    templates.Add(new AutoTemplate(type, ResourceTypeManager.GetInfo(type)));

            InitializeComponent();

            (Resources["ListSource"] as CollectionViewSource).Source = templates;
            Owner = App.Gui;
        }
    }
}
