using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaction logic for RichContentViewPage.xaml
    /// </summary>
    internal sealed partial class RichContentViewPage : System.Windows.Controls.Page
    {
        internal RichContentViewPage(ViewModel.RichContentViewPageModel model)
        {
            ModelClientHelper.InitializeModel(model);

            ResourceManager.Open(model.Resource);

            DataContext = model;
            CommandBindings.AddRange(model.Commands);

            InitializeComponent();
            frame.NavigateToString(model.Markup);
        }
    }
}
