using System.Windows.Controls;
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
            this.Editor.Text = resource.GetText();
        }

        public IEditable Resource { get; private set; }

        internal void Save() // to be called from ribbon
        {
            Resource.SaveText(Editor.Text);
        }
    }
}
