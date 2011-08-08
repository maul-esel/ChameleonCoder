using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources.Management;
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

        public int[] availableTranslations { get { return new int[2] { 1031, 1033 }; } }

        public BreadcrumbContext BreadcrumbRoot
        {
            get
            {
                return new BreadcrumbContext(ChameleonCoder.Properties.Resources.Item_Home,
                    new BitmapImage(new Uri("pack://application:,,,/Images/home.png")),
                    new BreadcrumbContext[]
                        {
                        new BreadcrumbContext(ChameleonCoder.Properties.Resources.Item_List,
                            new BitmapImage(new Uri("pack://application:,,,/Images/list.png")),
                            ResourceManager.GetChildren(),
                            true, false),
                        new BreadcrumbContext(ChameleonCoder.Properties.Resources.Item_Settings,
                            new BitmapImage(new Uri("pack://application:,,,/Images/RibbonTab1/settings.png")),
                            null,
                            false, true)
                        });
            }
        }

        #region localization
        public string Title { get { return "CC - ChameleonCoder alpha 1"; } }

        public string Item_Home { get { return Res.Item_Home; } }
        public string Item_List { get { return Res.Item_List; } }
        public string Item_Settings { get { return Res.Item_Settings; } }

        public string Info_Name { get { return Res.Info_Name; } }
        public string Info_Description { get { return Res.Info_Description; } }
        public string Info_GUID { get { return Res.Info_GUID; } }
        public string Info_Icon { get { return Res.Info_Icon; } }
        public string Info_Special { get { return Res.Info_Special; } }
        public string Info_Author { get { return Res.Info_Author; } }
        public string Info_Visual { get { return Res.Info_Visual; } }
        public string Info_Alias { get { return Res.Info_Alias; } }
        public string Info_File { get { return Res.Info_File; } }
        public string Info_Assembly { get { return Res.Info_Assembly; } }
        public string Info_Class { get { return Res.Info_Class; } }
        public string Info_Version { get { return Res.Info_Version; } }

        public string TypeSelector_Select { get { return Res.TypeSelector_Select; } }

        public string Services { get { return Res.Services; } }

        public string Action_Restart { get { return Res.Action_Restart; } }
        public string Action_Exit { get { return Res.Action_Exit; } }
        public string Action_Apply { get { return Res.Action_Apply; } }
        public string Action_Add { get { return Res.Action_Add; } }
        public string Action_Remove { get { return Res.Action_Remove; } }
        public string Action_OK { get { return Res.Action_OK; } }
        public string Action_Cancel { get { return Res.Action_Cancel; } }

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
