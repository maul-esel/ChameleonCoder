using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources.Management;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder
{
    /*
     * TODO:
     * ============================================================
     * - implement INotifyPropertyChanged on all view model classes
     * - avoid re-instantiating the classes
     * - use InformationProvider.LanguageChanged event
     * - add a 'UpdateAll()' and a 'Update(string)' method
     */
    internal sealed class ViewModel
    {
        public ViewModel()
        {
            Tabs = new ObservableCollection<TabContext>();
        }

        public ObservableCollection<TabContext> Tabs { get; private set; }        

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
                            null, // todo: add image
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
        public static string Welcome { get { return Res.Welcome; } }
        public static string StartSelection { get { return Res.StartSelection; } }
        public static string Selection_List { get { return Res.Selection_List; } }
        public static string Selection_Settings { get { return Res.Selection_Settings; } }
        public static string Selection_CreateNew { get { return Res.Selection_CreateNew; } }
        public static string Selection_Plugins { get { return Res.Selection_Plugins; } }
        #endregion

        #endregion

        /// <summary>
        /// a class containing localization strings for the CCSearchReplace dialog
        /// </summary>
        internal sealed class SearchReplaceModel
        {
            public static string SearchFor { get { return Res.SR_SearchFor; } }

            public static string ReplaceBy { get { return Res.SR_ReplaceBy; } }

            public static string FindNext { get { return Res.SR_FindNext; } }

            public static string Replace { get { return Res.Edit_Replace; } }

            public static string ReplaceAll { get { return Res.SR_ReplaceAll; } }

            public static string Title { get { return Res.SR_Title; } }

            public static string MatchCase { get { return Res.SR_MatchCase; } }

            public static string MatchWholeWord { get { return Res.SR_MatchWholeWord; } }

            public static string WrapAround { get { return Res.SR_WrapAround; } }
        }

        /// <summary>
        /// a class containing localization strings and other data for the PluginPage class
        /// </summary>
        internal sealed class PluginPageModel
        {
            internal PluginPageModel(IList<Plugins.IPlugin> pluginList)
            {
                plugins = new ObservableCollection<Plugins.IPlugin>(pluginList);
            }

            public ObservableCollection<Plugins.IPlugin> PluginList
            {
                get { return plugins; }
            }

            private readonly ObservableCollection<Plugins.IPlugin> plugins;

            public static string Types_All { get { return Res.PP_TypesAll; } }

            public static string Types_Templates { get { return Res.PP_TypesTemplate; } }

            public static string Types_Services { get { return Res.PP_TypesService; } }

            public static string Types_LanguageModules { get { return Res.PP_TypesLanguageModule; } }

            public static string Types_ResourceFactories { get { return Res.PP_TypesResourceFactory; } }

            public static string Uninstall { get { return Res.PP_Uninstall; } }

            public static string Install { get { return Res.PP_Install; } }
        }

        /// <summary>
        /// a class containing localization strings and other data for the ResourceViewPage class
        /// </summary>
        internal sealed class ResourceViewPageModel
        {
            internal ResourceViewPageModel(Resources.Interfaces.IResource resource)
            {
                resourceInstance = resource;
            }

            public Resources.Interfaces.IResource Resource { get { return resourceInstance; } }

            private readonly Resources.Interfaces.IResource resourceInstance;

            public IDictionary<string, string> MetaData { get { return resourceInstance.GetMetadata(); } }

            public static string MetaDataKey { get { return Res.VP_MetaDataKey; } }

            public static string MetaDataValue { get { return Res.VP_MetaDataValue; } }
        }

        /// <summary>
        /// a class containing localization strings and other data for the SettingsPage class
        /// </summary>
        internal sealed class SettingsPageModel
        {
            public static int[] availableTranslations { get { return new int[2] { 1031, 1033 }; } }

            public static FontFamily CodeFont
            {
                get { return family; }
                set { Settings.ChameleonCoderSettings.Default.CodeFont = (family = value).Source; }
            }

            private static FontFamily family = new FontFamily(Settings.ChameleonCoderSettings.Default.CodeFont);

            public static int CodeFontSize
            {
                get { return Settings.ChameleonCoderSettings.Default.CodeFontSize; }
                set { Settings.ChameleonCoderSettings.Default.CodeFontSize = value; }
            }

            public static int UILanguage
            {
                get { return Settings.ChameleonCoderSettings.Default.Language; }

                set
                {
                    Res.Culture =
                        new System.Globalization.CultureInfo(
                            Settings.ChameleonCoderSettings.Default.Language = value
                            );

                    App.Gui.DataContext = new ViewModel() { Tabs = App.Gui.MVVM.Tabs };
                    App.Gui.breadcrumb.Path =
                        App.Gui.breadcrumb.PathFromBreadcrumbItem(App.Gui.breadcrumb.RootItem)
                        + "/" + Item_Settings;

                    Interaction.InformationProvider.OnLanguageChanged();
                }
            }

            public static string Item_Settings { get { return Res.Item_Settings; } }

            public static string Setting_Language { get { return Res.SP_Language; } }

            public static string Setting_EnableUpdate { get { return Res.SP_Update; } }

            public static string Setting_InstallExt { get { return Res.SP_InstallExt; } }

            public static string Setting_InstallCOM { get { return Res.SP_InstallCOM; } }

            public static string Setting_CodeFont { get { return Res.SP_CodeFont; } }

            public static string Setting_CodeFontSize { get { return Res.SP_CodeFontSize; } }
        }
    }
}
