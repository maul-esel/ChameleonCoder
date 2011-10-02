using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Update("Resource");
            }
        }

        private IComponent resourceInstance;

        #region localization

        public string Title { get { return MainWindowModel.Title; } }

        public string Info_Name { get { return Res.Info_Name; } }

        public string Info_GUID { get { return Res.Info_Identifier; } }

        public string Info_Icon { get { return Res.Info_Icon; } }

        public string Info_Description { get { return Res.Info_Description; } }

        public string Info_Special { get { return Res.Info_Special; } }

        public string Action_Add { get { return Res.Action_Add; } }

        public string Action_Remove { get { return Res.Action_Remove; } }

        public string Action_Cancel { get { return Res.Action_Cancel; } }

        public string Action_OK { get { return Res.Action_OK; } }

        #endregion
    }
}
