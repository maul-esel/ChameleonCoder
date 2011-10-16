using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(Navigation.ResourceListPage))]
    internal sealed class ResourceListPageModel : ViewModelBase
    {
        private ResourceListPageModel()
        {
        }

        internal static ResourceListPageModel Instance
        {
            get
            {
                lock (modelInstance)
                {
                    return modelInstance;
                }
            }
        }

        private static readonly ResourceListPageModel modelInstance = new ResourceListPageModel();
        
        public static string Info_Name { get { return Res.Info_Name; } }

        public static string Info_Description { get { return Res.Info_Description; } }
    }
}
