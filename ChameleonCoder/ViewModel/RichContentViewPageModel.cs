using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(Navigation.RichContentViewPage))]
    internal sealed class RichContentViewPageModel : ViewModelBase
    {
        internal RichContentViewPageModel(IRichContentResource resource)
        {
            Resource = resource;
        }

        internal IRichContentResource Resource { get; private set; }

        public string Markup
        {
            get { return Resource.GetHtml(); }
        }
    }
}
