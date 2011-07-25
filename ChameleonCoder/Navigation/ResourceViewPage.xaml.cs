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
            this.DataContext = resource;
            InitializeComponent();
            (this.Resources["ResPropList"] as CollectionViewSource).Source = resource;
            this.MetadataGrid.ItemsSource = resource.MetaData;
            this.Resource = resource;
            ResourceManager.ActiveItem = resource;
        }

        public IResource Resource { get; private set; }
    }
}
