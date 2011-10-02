using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
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

        public string Info_Name { get { return Res.Info_Name; } }

        public string Info_Description { get { return Res.Info_Description; } }
    }
}
