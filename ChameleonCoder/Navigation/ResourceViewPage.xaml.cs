using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für ResourceViewPage.xaml
    /// </summary>
    public partial class ResourceViewPage : Page
    {
        public ResourceViewPage(IResource resource)
        {
            this.DataContext = this.Resource = ResourceManager.ActiveItem = resource;
            InitializeComponent();
        }

        public IResource Resource { get; private set; }
    }
}
