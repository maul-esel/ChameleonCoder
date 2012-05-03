using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.UI.ViewModel
{
    [DefaultRepresentation(typeof(Navigation.ResourceListPage))]
    internal sealed class ResourceListPageModel : ViewModelBase
    {
        internal ResourceListPageModel(IChameleonCoderApp app)
            : base(app)
        {
        }
        
        public static string Info_Name { get { return Res.Info_Name; } }

        public static string Info_Description { get { return Res.Info_Description; } }
    }
}
