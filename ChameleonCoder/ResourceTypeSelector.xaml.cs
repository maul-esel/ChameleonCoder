using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ChameleonCoder.Resources.Management;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für ResourceTypeSelector.xaml
    /// </summary>
    public partial class ResourceTypeSelector : Window
    {
        public Type SelectedResult = null;

        public ResourceTypeSelector() : this(ResourceTypeManager.GetResourceTypes()) { }

        public ResourceTypeSelector(IEnumerable<Type> types)
        {
            InitializeComponent();
            this.Closed += (sender, e) => { if (DialogResult == true) SelectedResult = ((KeyValuePair<Brush, Type>)typeList.SelectedItem).Value; };

            Dictionary<Brush, Type> list = new Dictionary<Brush, Type>();
            foreach (Type t in types)
                list.Add(ResourceTypeManager.GetInfo(t).Background, t);

            DataContext = list.OrderBy(pair => ResourceTypeManager.GetInfo(pair.Value).DisplayName);
        }

        private void UpdateInfo(object sender, EventArgs e)
        {
            if (this.IsInitialized && typeList.SelectedItem != null)
            {
                Type type = ((KeyValuePair<Brush, Type>)typeList.SelectedItem).Value;
                ChameleonCoder.Resources.ResourceTypeInfo info = ResourceTypeManager.GetInfo(type);


                Info.DataContext = new
                {
                    Name = info.DisplayName,
                    Author = info.Author,
                    Icon = info.TypeIcon,
                    Color = info.Background,
                    Alias = info.Alias,
                    File = System.IO.Path.GetFileName(type.Assembly.Location),
                    Assembly = type.Assembly.GetName().Name,
                    Class = type.Name + " (" + type + ")"
                };
            }
            else
                Info.DataContext = null;
        }
    }
}
