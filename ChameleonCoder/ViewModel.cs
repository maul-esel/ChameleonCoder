using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    internal sealed class ViewModel
    {
        internal ViewModel()
        {
            Tabs = new ObservableCollection<TabContext>();
        }

        public ObservableCollection<TabContext> Tabs { get; set; }

        public string Item_Home { get { return Res.Item_Home; } }
        public string Item_List { get { return Res.Item_List; } }
        public string Item_Settings { get { return Res.Item_Settings; } }

        public string Services { get { return Res.Services; } }

        public string Action_Restart { get { return Res.Action_Restart; } }
        public string Action_Exit { get { return Res.Action_Exit; } }

        public string ResourceImport { get { return Res.ResourceImport; } }
        public string ResourcePackage { get { return Res.ResourcePackage; } }
        public string ResourceUnpackage { get { return Res.ResourceUnpackage; } }

        public string Help { get { return Res.Help; } }
        public string About { get { return Res.About; } }

        #region SettingsPage
        public string Setting_Language { get { return Res.Setting_Language; } }
        public string Setting_EnableUpdate { get { return Res.Setting_EnableUpdate; } }
        public string Setting_ProgrammingDir { get { return Res.Setting_ProgrammingDir; } }
        public string Setting_SelectProgDir { get { return Res.Setting_SelectProgDir; } }
        public string Setting_InstallExt { get { return Res.Setting_InstallExt; } }
        public string Setting_InstallCOM { get { return Res.Setting_InstallCOM; } }
        #endregion

        #region WelcomePage
        public string Welcome { get { return Res.Welcome; } }
        public string StartSelection { get { return Res.StartSelection; } }
        public string Selection_List { get { return Res.Selection_List; } }
        public string Selection_Settings { get { return Res.Selection_Settings; } }
        public string Selection_CreateNew { get { return Res.Selection_CreateNew; } }
        #endregion
    }
}
