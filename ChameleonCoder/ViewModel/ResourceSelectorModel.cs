using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class ResourceSelectorModel : ViewModelBase
    {
        public IComponent Resource
        {
            get { return resourceInstance; }
            internal set
            {
                resourceInstance = value;
                OnPropertyChanged("Resource");
            }
        }

        public bool ShowReferences
        {
            get;
            internal set;
        }

        public Resources.ResourceCollection ResourceList
        {
            get;
            internal set;
        }

        private IComponent resourceInstance;

        #region localization

        public static string Title { get { return MainWindowModel.Title; } }

        public static string Info_Name { get { return Res.Info_Name; } }

        public static string Info_GUID { get { return Res.Info_Identifier; } }

        public static string Info_Icon { get { return Res.Info_Icon; } }

        public static string Info_Description { get { return Res.Info_Description; } }

        public static string Info_Special { get { return Res.Info_Special; } }

        public static string Action_Add { get { return Res.Action_Add; } }

        public static string Action_Remove { get { return Res.Action_Remove; } }

        public static string Action_Cancel { get { return Res.Action_Cancel; } }

        public static string Action_OK { get { return Res.Action_OK; } }

        #endregion
    }
}
