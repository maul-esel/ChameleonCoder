using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ChameleonCoder.Navigation; // avoid
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Shared;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class MainWindowModel : ViewModelBase
    {
        private MainWindowModel()
        {
            Commands.Add(new CommandBinding(ApplicationCommands.Close,
                CloseCommandExecuted));

            Commands.Add(new CommandBinding(NavigationCommands.BrowseHome,
                GoHomeCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenResourceListPage,
                OpenResourceListPageCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenPluginPage,
                OpenPluginPageCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenSettingsPage,
                OpenSettingsPageCommandExecuted));

            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenNewTab,
                OpenNewTabCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.CloseTab,
                CloseTabCommandExecuted));

            Commands.Add(new CommandBinding(ChameleonCoderCommands.ExecuteService,
                ExecuteServiceCommandExecuted));

            Commands.Add(new CommandBinding(ChameleonCoderCommands.CreateResource,
                CreateResourceCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenResourceView,
                OpenResourceViewCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenResourceEdit,
                OpenResourceEditCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.DeleteResource,
                DeleteResourceCommandExecuted));

            PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "ActiveTab")
                        OnViewChanged();
                };

            OpenNewTab();
        }

        #region singleton

        /// <summary>
        /// gets the instance of this model
        /// </summary>
        internal static MainWindowModel Instance
        {
            get
            {
                lock (modelInstance)
                {
                    return modelInstance;
                }
            }
        }

        private static readonly MainWindowModel modelInstance = new MainWindowModel();

        #endregion

        #region commanding

        /// <summary>
        /// implements the logic for the ApplicationCommands.Close command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            Close(e.Parameter as string == "restart");
        }        

        /// <summary>
        /// implements the logic for the NavigationCommands.BrowseHome command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void GoHomeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            GoHome();            
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.OpenResourceList command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void OpenResourceListPageCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenResourceList();
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.OpenPluginPage command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void OpenPluginPageCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenPluginPage();
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.OpenSettingsPage command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void OpenSettingsPageCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenSettingsPage();
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.OpenNewTab command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void OpenNewTabCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenNewTab();
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.CloseTab command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void CloseTabCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            var item = e.Parameter as TabContext;
            if (item != null)
                CloseTab(item);
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.ExecuteService command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void ExecuteServiceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            var service = e.Parameter as Plugins.IService;
            if (service != null)
                Plugins.PluginManager.CallService(service.Identifier);
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.CreateResource command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void CreateResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            var resource = e.Parameter as IResource;
            NewResourceDialog dialog = new NewResourceDialog(resource);
            dialog.ShowDialog();
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.OpenResourceView command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void OpenResourceViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            var resource = e.Parameter as IResource;
            if (resource == null)
            {
                var reference = e.Parameter as Resources.ResourceReference;
                if (reference != null)
                    resource = reference.Resolve();
            }
            if (resource == null)
                throw new ArgumentException("resource to open is null or not an IResource instance");

            OpenResourceView(resource);
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.OpenResourceEdit command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void OpenResourceEditCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            var resource = e.Parameter as IEditable;
            if (resource == null)
            {
                var reference = e.Parameter as Resources.ResourceReference;
                if (reference != null)
                    resource = reference.Resolve() as IEditable;
            }
            if (resource == null)
                throw new ArgumentException("resource to open is null or not an IEditable instance");

            OpenResourceEdit(resource);
        }

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.DeleteResource command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">data related to the command execution</param>
        private void DeleteResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;

            var resource = e.Parameter as IResource;
            if (resource == null)
            {
                var reference = e.Parameter as Resources.ResourceReference;
                if (reference != null)
                    resource = reference.Resolve() as IResource;
            }
            if (resource == null)
                throw new ArgumentException("resource to open is null or not an IResource instance");

            DeleteResource(resource);
        }

        #endregion

        private static void Close(bool restart)
        {
            if (restart)
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);

            App.Current.Shutdown(0);
        }

        #region page management

        private void GoHome()
        {
            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.Home;
            context.Content = new WelcomePage();

            OnViewChanged();
            BreadcrumbPath = BreadcrumbRoot.Name;
        }

        private void OpenResourceList()
        {
            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.ResourceList;
            context.Content = new ResourceListPage();

            OnViewChanged();
            BreadcrumbPath = string.Format("{1}{0}{2}",
                        BreadcrumbSeparator,
                        BreadcrumbRoot.Name,
                        Res.Item_List);
        }

        private void OpenPluginPage()
        {
            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.Plugins;
            context.Content = new PluginPage();

            OnViewChanged();
            BreadcrumbPath = string.Format("{1}{0}{2}",
                        BreadcrumbSeparator,
                        BreadcrumbRoot.Name,
                        Res.Item_Plugins);
        }

        private void OpenSettingsPage()
        {
            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.Settings;
            context.Content = new SettingsPage();

            OnViewChanged();
            BreadcrumbPath = string.Format("{1}{0}{2}",
                        BreadcrumbSeparator,
                        BreadcrumbRoot.Name,
                        Item_Settings);
        }

        #endregion

        #region resources

        private void OpenResourceView(IResource resource)
        {
            var context = ActiveTab;
            context.Resource = resource;
            context.Type = CCTabPage.ResourceView;
            context.Content = new ResourceViewPage(resource);

            OnViewChanged();
            BreadcrumbPath = string.Format("{1}{0}{2}{3}",
                        BreadcrumbSeparator,
                        BreadcrumbRoot.Name,
                        Res.Item_List,
                        ResourceManager.ActiveItem.GetPath(BreadcrumbSeparator));
        }

        private void OpenResourceEdit(IEditable resource)
        {
            var context = ActiveTab;
            context.Resource = resource;
            context.Type = CCTabPage.ResourceEdit;
            context.Content = new EditPage(resource);

            OnViewChanged();
            BreadcrumbPath = string.Format("{1}{0}{2}{3}",
                        BreadcrumbSeparator,
                        BreadcrumbRoot.Name,
                        Res.Item_List,
                        ResourceManager.ActiveItem.GetPath(BreadcrumbSeparator));
        }

        private void DeleteResource(IResource resource)
        {
            if (OnConfirm(Res.Status_DeleteResource, string.Format(Res.Del_Confirm, resource.Name)) == true)            
            {
                resource.Delete();
            }
        }

        #endregion

        #region breadcrumb

        public static string BreadcrumbSeparator
        {
            get { return "/"; }
        }

        private string breadcrumbPath;

        public string BreadcrumbPath
        {
            get { return breadcrumbPath; }
            set
            {
                breadcrumbPath = value;
                OnPropertyChanged("BreadcrumbPath");
            }
        }

        public static BreadcrumbContext BreadcrumbRoot
        {
            get
            {
                return new BreadcrumbContext(new BitmapImage(new Uri("pack://application:,,,/Images/home.png")),
                    new BreadcrumbContext[3]
                        {
                        new BreadcrumbContext(new BitmapImage(new Uri("pack://application:,,,/Images/list.png")),
                            ResourceManager.GetChildren(),
                            CCTabPage.ResourceList),
                        new BreadcrumbContext(new BitmapImage(new Uri("pack://application:,,,/Images/RibbonTab1/settings.png")),
                            null,
                            CCTabPage.Settings),
                        new BreadcrumbContext(new BitmapImage(new Uri("pack://application:,,,/Images/plugins.png")),
                            null,
                            CCTabPage.Plugins)
                        },
                    CCTabPage.Home);
            }
        }

        #endregion

        #region tabs

        public ObservableCollection<TabContext> Tabs
        {
            get { return tabCollection; }
        }

        private readonly ObservableCollection<TabContext> tabCollection = new ObservableCollection<TabContext>();

        public TabContext ActiveTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                OnPropertyChanged("ActiveTab");
            }
        }

        private TabContext selectedTab;

        private void OpenNewTab()
        {
            var context = new TabContext(CCTabPage.Home, new WelcomePage());
            Tabs.Add(context);
            ActiveTab = context;
        }

        private void CloseTab(TabContext item)
        {
            ChameleonCoderCommands.SaveResource.Execute(null, item.Content as System.Windows.IInputElement);
            Tabs.Remove(item);
        }

        #endregion

        #region services

        public static bool EnableServices
        {
            get { return Plugins.PluginManager.ServiceCount > 0; }
        }

        public static IEnumerable<Plugins.IService> ServiceList
        {
            get
            {
                return Plugins.PluginManager.GetServices();
            }
        }

        #endregion

        #region resource types

        public static IEnumerable<Type> LoadedResourceTypes
        {
            get { return ResourceTypeManager.GetResourceTypes(); }
        }

        #endregion

        #region localization
        public static string Title { get { return "CC - ChameleonCoder alpha 2"; } }

        public static string Item_Home { get { return Res.Item_Home; } }
        public static string Item_Settings { get { return Res.Item_Settings; } }

        public static string Selection_CreateNew { get { return Res.WP_CreateResource; } }

        public static string Services { get { return Res.Services; } }

        public static string Action_Restart { get { return Res.Action_Restart; } }
        public static string Action_Exit { get { return Res.Action_Exit; } }

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

        #endregion

        #region events

        internal event EventHandler<Interaction.ViewChangedEventArgs> ViewChanged;

        private void OnViewChanged()
        {
            var handler = ViewChanged;
            if (handler != null)
            {
                handler(this, new Interaction.ViewChangedEventArgs(ActiveTab));
            }
        }

        #endregion
    }
}
