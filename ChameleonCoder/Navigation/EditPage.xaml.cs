using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public EditPage(IEditable resource)
        {
            InitializeComponent();
            this.Resource = resource;
            MessageBox.Show(resource.Name);
            this.Editor.Text = resource.GetText();
        }

        public IEditable Resource { get; private set; }
    }
}
