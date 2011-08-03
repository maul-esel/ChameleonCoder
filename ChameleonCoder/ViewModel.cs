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

        #region localization
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

        #region RibbonTab headers
        public string RibbonContext_List { get { return Res.RibbonContext_List; } }
        public string RibbonContext_Edit { get { return Res.RibbonContext_Edit; } }
        public string RibbonContext_View { get { return Res.RibbonContext_View; } }
        #endregion

        #region ResourceList Ribbon
        public string List_EnableGroups { get { return Res.List_EnableGroups; } }
        public string List_ShowResolvable { get { return Res.List_ShowResolvable; } }
        public string List_HiddenTypes { get { return Res.List_HiddenTypes; } }
        #endregion

        #region ResourceEdit Ribbon
        public string RibbonGroup_Clipboard { get { return Res.RibbonGroup_Clipboard; } }
        public string RibbonGroup_View { get { return Res.RibbonGroup_View; } }
        public string RibbonGroup_UndoRedo { get { return Res.RibbonGroup_UndoRedo; } }
        public string RibbonGroup_SaveOpen { get { return Res.RibbonGroup_SaveOpen; } }
        public string RibbonGroup_Search { get { return Res.RibbonGroup_Search; } }
        public string RibbonGroup_Code { get { return Res.RibbonGroup_Code; } }

        public string Edit_Paste { get { return Res.Edit_Paste; } }
        public string Edit_Cut { get { return Res.Edit_Cut; } }
        public string Edit_Copy { get { return Res.Edit_Copy; } }

        public string Edit_ZoomIn { get { return Res.Edit_ZoomIn; } }
        public string Edit_ZoomOut { get { return Res.Edit_ZoomOut; } }

        public string Edit_Undo { get { return Res.Edit_Undo; } }
        public string Edit_Redo { get { return Res.Edit_Redo; } }

        public string Edit_Save { get { return Res.Edit_Save; } }

        public string Edit_Search { get { return Res.Edit_Search; } }
        public string Edit_Replace { get { return Res.Edit_Replace; } }

        public string Edit_NewCodeStub { get { return Res.Edit_NewCodeStub; } }
        #endregion

        #region ResourceView Ribbon
        public string View_AddChild { get { return Res.View_AddChild; } }
        public string View_Move { get { return Res.View_Move; } }
        public string View_Delete { get { return Res.View_Delete; } }

        public string View_Edit { get { return Res.View_Edit; } }
        #endregion

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

        #endregion
    }
}
