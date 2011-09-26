using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources.Management;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class MainWindowModel : ViewModelBase
    {
        public ObservableCollection<TabContext> Tabs
        {
            get { return tabCollection; }
        }

        private readonly ObservableCollection<TabContext> tabCollection = new ObservableCollection<TabContext>();

        public static BreadcrumbContext BreadcrumbRoot
        {
            get
            {
                return new BreadcrumbContext(Item_Home,
                    new BitmapImage(new Uri("pack://application:,,,/Images/home.png")),
                    new BreadcrumbContext[3]
                        {
                        new BreadcrumbContext(Item_List,
                            new BitmapImage(new Uri("pack://application:,,,/Images/list.png")),
                            ResourceManager.GetChildren(),
                            BreadcrumbContext.ContextType.ResourceList),
                        new BreadcrumbContext(Item_Settings,
                            new BitmapImage(new Uri("pack://application:,,,/Images/RibbonTab1/settings.png")),
                            null,
                            BreadcrumbContext.ContextType.Settings),
                        new BreadcrumbContext(Item_Plugins,
                            new BitmapImage(new Uri("pack://application:,,,/Images/plugins.png")),
                            null,
                            BreadcrumbContext.ContextType.Plugins)
                        },
                    BreadcrumbContext.ContextType.Home);
            }
        }

        #region localization
        public static string Title { get { return "CC - ChameleonCoder alpha 2"; } }

        public static string Item_Home { get { return Res.Item_Home; } }
        public static string Item_List { get { return Res.Item_List; } }
        public static string Item_Settings { get { return Res.Item_Settings; } }
        public static string Item_Plugins { get { return Res.Item_Plugins; } }

        public static string Info_Name { get { return Res.Info_Name; } }
        public static string Info_Description { get { return Res.Info_Description; } }
        public static string Info_GUID { get { return Res.Info_Identifier; } }
        public static string Info_Icon { get { return Res.Info_Icon; } }
        public static string Info_Special { get { return Res.Info_Special; } }
        public static string Info_Author { get { return Res.Info_Author; } }
        public static string Info_Visual { get { return Res.Info_Visual; } }
        public static string Info_Alias { get { return Res.Info_Alias; } }
        public static string Info_File { get { return Res.Info_File; } }
        public static string Info_Assembly { get { return Res.Info_Assembly; } }
        public static string Info_Class { get { return Res.Info_Class; } }
        public static string Info_Version { get { return Res.Info_Version; } }

        public static string TypeSelector_Select { get { return Res.TypeSelector_Select; } }

        public static string Services { get { return Res.Services; } }

        public static string Action_Restart { get { return Res.Action_Restart; } }
        public static string Action_Exit { get { return Res.Action_Exit; } }
        public static string Action_Apply { get { return Res.Action_Apply; } }
        public static string Action_Add { get { return Res.Action_Add; } }
        public static string Action_Remove { get { return Res.Action_Remove; } }
        public static string Action_OK { get { return Res.Action_OK; } }
        public static string Action_Cancel { get { return Res.Action_Cancel; } }

        public static string Help { get { return Res.Help; } }
        public static string About { get { return Res.About; } }

        public static string Meta_Add { get { return Res.Meta_Add; } }
        public static string Meta_Delete { get { return Res.Meta_Delete; } }

        #region RibbonTab headers
        public static string RibbonContext_List { get { return Res.RibbonContext_List; } }
        public static string RibbonContext_Edit { get { return Res.RibbonContext_Edit; } }
        public static string RibbonContext_View { get { return Res.RibbonContext_View; } }
        #endregion

        #region ResourceList Ribbon
        public static string List_EnableGroups { get { return Res.List_EnableGroups; } }
        public static string List_HiddenTypes { get { return Res.List_HiddenTypes; } }
        public static string List_SortResources { get { return Res.List_SortResources; } }
        #endregion

        #region ResourceEdit Ribbon
        public static string RibbonGroup_Clipboard { get { return Res.RibbonGroup_Clipboard; } }
        public static string RibbonGroup_View { get { return Res.RibbonGroup_View; } }
        public static string RibbonGroup_UndoRedo { get { return Res.RibbonGroup_UndoRedo; } }
        public static string RibbonGroup_SaveOpen { get { return Res.RibbonGroup_SaveOpen; } }
        public static string RibbonGroup_Search { get { return Res.RibbonGroup_Search; } }
        public static string RibbonGroup_Code { get { return Res.RibbonGroup_Code; } }

        public static string Edit_Paste { get { return Res.Edit_Paste; } }
        public static string Edit_Cut { get { return Res.Edit_Cut; } }
        public static string Edit_Copy { get { return Res.Edit_Copy; } }

        public static string Edit_ZoomIn { get { return Res.Edit_ZoomIn; } }
        public static string Edit_ZoomOut { get { return Res.Edit_ZoomOut; } }

        public static string Edit_Undo { get { return Res.Edit_Undo; } }
        public static string Edit_Redo { get { return Res.Edit_Redo; } }

        public static string Edit_Save { get { return Res.Edit_Save; } }

        public static string Edit_Search { get { return Res.Edit_Search; } }
        public static string Edit_Replace { get { return Res.Edit_Replace; } }

        public static string Edit_NewCodeStub { get { return Res.Edit_NewCodeStub; } }
        #endregion

        #region ResourceView Ribbon
        public static string View_AddChild { get { return Res.View_AddChild; } }
        public static string View_Move { get { return Res.View_Move; } }
        public static string View_Delete { get { return Res.View_Delete; } }
        public static string View_Copy { get { return Res.View_Copy; } }

        public static string View_Edit { get { return Res.View_Edit; } }
        #endregion

        #region WelcomePage
        public static string Welcome { get { return Res.WP_Welcome; } }
        public static string StartSelection { get { return Res.WP_StartSelection; } }
        public static string Selection_List { get { return Res.WP_GoList; } }
        public static string Selection_Settings { get { return Res.WP_GoSettings; } }
        public static string Selection_CreateNew { get { return Res.WP_CreateResource; } }
        public static string Selection_Plugins { get { return Res.WP_GoPlugins; } }
        #endregion

        #endregion
    }
}
