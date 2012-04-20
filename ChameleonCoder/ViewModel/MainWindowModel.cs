using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Shared;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(MainWindow))]
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
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenFileManagementPage,
                OpenFileManagementPageCommandExecuted));

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
            Commands.Add(new CommandBinding(ChameleonCoderCommands.CopyResource,
                CopyResourceCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.MoveResource,
                MoveResourceCommandExecuted));

            Commands.Add(new CommandBinding(ChameleonCoderCommands.CloseFiles,
                CloseFilesCommandExecuted,
                (s, e) => e.CanExecute = ChameleonCoderApp.RunningObject.FileMan.Files.Count > 0));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.OpenFile,
                OpenFileCommandExecuted));
            Commands.Add(new CommandBinding(ChameleonCoderCommands.CreateFile,
                CreateFileCommandExecuted));
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
                ChameleonCoderApp.RunningObject.PluginMan.CallService(service.Identifier);
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

        private void CopyResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var resource = e.Parameter as IResource;
            if (resource == null)
            {
                var reference = e.Parameter as Resources.ResourceReference;
                if (reference != null)
                    resource = reference.Resolve() as IResource;
            }
            if (resource == null)
                throw new ArgumentException("resource to copy is null or not an IResource instance");

            CopyResource(resource);
        }

        private void MoveResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var resource = e.Parameter as IResource;
            if (resource == null)
            {
                var reference = e.Parameter as Resources.ResourceReference;
                if (reference != null)
                    resource = reference.Resolve() as IResource;
            }
            if (resource == null)
                throw new ArgumentException("resource to move is null or not an IResource instance");

            MoveResource(resource);
        }

        private void OpenFileManagementPageCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenFileManagementPage();
        }

        private void CloseFilesCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            CloseFiles();
        }

        private void OpenFileCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpenFile();
        }

        private void CreateFileCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            CreateFile();
        }

        #endregion

        private static void Close(bool restart)
        {
            if (restart)
                System.Diagnostics.Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);

            ChameleonCoderApp.RunningObject.Exit(0);
        }

        #region page management

        private void GoHome()
        {
            var args = OnRepresentationNeeded(WelcomePageModel.Instance, false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.Home;
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void OpenResourceList()
        {
            var args = OnRepresentationNeeded(ResourceListPageModel.Instance, false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.ResourceList;
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void OpenPluginPage()
        {
            var args = OnRepresentationNeeded(PluginPageModel.Instance, false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.Plugins;            
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void OpenSettingsPage()
        {
            var args = OnRepresentationNeeded(SettingsPageModel.Instance, false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.Settings;
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void OpenFileManagementPage()
        {
            var args = OnRepresentationNeeded(new FileManagementPageModel(), false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = null;
            context.Type = CCTabPage.FileManagement;
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        #endregion

        #region resources

        private void OpenResourceView(IResource resource)
        {
            var args = OnRepresentationNeeded(new ResourceViewPageModel(resource), false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = resource;
            context.Type = CCTabPage.ResourceView;
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void OpenResourceEdit(IEditable resource)
        {
            var args = OnRepresentationNeeded(new EditPageModel(resource), false);
            if (args.Cancel)
                return;

            var context = ActiveTab;
            context.Resource = resource;
            context.Type = CCTabPage.ResourceEdit;
            context.Content = args.Representation;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void DeleteResource(IResource resource)
        {
            if (OnConfirm(Res.Status_DeleteResource, string.Format(Res.Del_Confirm, resource.Name)) == true)            
            {
                resource.Delete();
            }
        }

        private void CopyResource(IResource resource)
        {
            var model = new ViewModel.ResourceSelectorModel(resource.File.App.ResourceMan.GetChildren(), 1) { ShowReferences = false };
            var args = OnRepresentationNeeded(model, true);

            if (args.Cancel)
                return;

            if (model.SelectedResources.Count > 0
                && model.SelectedResources[0] != resource)
            {
                resource.Copy(model.SelectedResources[0] as IResource);
            }
        }

        private void MoveResource(IResource resource)
        {
            var model = new ViewModel.ResourceSelectorModel(resource.File.App.ResourceMan.GetChildren(), 1) { ShowReferences = false };
            var args = OnRepresentationNeeded(model, true);

            if (args.Cancel)
                return;

            IResource newParent = model.SelectedResources[0] as IResource;

            if (model.SelectedResources.Count > 0 // user selected 1 resource
                && newParent != null) // resource is not null
            {
                if (!resource.File.App.ResourceMan.IsDescendantOf(resource, newParent) // can't be moved to descendant
                    && newParent != resource.Parent) // resource is not already parent
                {
                    resource.Move(newParent);
                }
                else
                {
                    OnReport(Res.Status_Move,
                        string.Format(Properties.Resources.Error_MoveToDescendant, resource.Name, model.SelectedResources[0].Name),
                        Interaction.MessageSeverity.Critical);
                }
            }
        }

        #endregion

        #region breadcrumb

        [System.ComponentModel.NotifyParentProperty(false)]
        public static string BreadcrumbSeparator
        {
            get { return ChameleonCoderApp.resourcePathSeparator; }
        }

        public string BreadcrumbPath
        {
            get
            {
                if (ActiveTab != null)
                    return ActiveTab.Path;
                return null;
            }
            set
            {
                /* TODO: switch active tab (move that from MainWindow) */
                OnPropertyChanged("BreadcrumbPath");
            }
        }

        public static BreadcrumbContext BreadcrumbRoot
        {
            get
            {
                return new BreadcrumbContext(new Uri("pack://application:,,,/Images/home.png"),
                    new BreadcrumbContext[4]
                        {
                        new BreadcrumbContext(new Uri("pack://application:,,,/Images/list.png"),
                            ChameleonCoderApp.RunningObject.ResourceMan.GetChildren(),
                            CCTabPage.ResourceList),
                        new BreadcrumbContext(new Uri("pack://application:,,,/Images/RibbonTab1/settings.png"),
                            null,
                            CCTabPage.Settings),
                        new BreadcrumbContext(new Uri("pack://application:,,,/Images/plugins.png"),
                            null,
                            CCTabPage.Plugins),
                        new BreadcrumbContext(new Uri("pack://application:,,,/Images/files.png"),
                            null,
                            CCTabPage.FileManagement)
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
                OnPropertyChanged("BreadcrumbPath");
                OnViewChanged();
            }
        }

        private TabContext selectedTab;

        private void OpenNewTab()
        {
            var context = new TabContext(CCTabPage.Home, OnRepresentationNeeded(WelcomePageModel.Instance, false));
            Tabs.Add(context);
            ActiveTab = context;

            OnViewChanged();
            OnPropertyChanged("BreadcrumbPath");
        }

        private void CloseTab(TabContext item)
        {
            ChameleonCoderCommands.SaveResource.Execute(null, item.Content as System.Windows.IInputElement);
            Tabs.Remove(item);

            if (Tabs.Count == 0)
            {
                OpenNewTab();
            }
        }

        #endregion

        #region services

        public static bool EnableServices
        {
            get { return ChameleonCoderApp.RunningObject.PluginMan.ServiceCount > 0; }
        }

        public static IEnumerable<Plugins.IService> ServiceList
        {
            get
            {
                return ChameleonCoderApp.RunningObject.PluginMan.GetServices();
            }
        }

        #endregion

        #region resource types

        public static IEnumerable<Type> LoadedResourceTypes
        {
            get { return ResourceTypeManager.GetResourceTypes(); }
        }

        #endregion

        #region files

        private void CloseFiles()
        {
            if (OnConfirm(Res.Status_ClosingFiles, Res.File_ConfirmClose) == true)
            {
                ChameleonCoderApp.RunningObject.ResourceMan.RemoveAll();
                ChameleonCoderApp.RunningObject.FileMan.CloseAll();
                XmlNamespaceManagerFactory.ClearManagers();
            }
        }

        private void OpenFile()
        {
            var args = OnSelectFile(Res.Status_OpeningFile + " " + Res.File_SelectOpen,
                Environment.CurrentDirectory, true);

            if (args.Cancel)
                return;

            var path = args.Path;
            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path))
            {
                OnReport(Res.Status_OpeningFile, string.Format(Res.Error_InvalidFile, path),
                    Interaction.MessageSeverity.Critical);
                return;
            }

            OpenFile(path);
        }


        private void OpenFile(string path)
        {
            if (ChameleonCoderApp.RunningObject.FileMan.IsOpen(path))
            {
                OnReport(Res.Status_OpeningFile, string.Format(Res.Error_FileAlreadyLoaded, path),
                    Interaction.MessageSeverity.Critical);
                return;
            }

            ChameleonCoderApp.RunningObject.FileMan.Open(path);
            ChameleonCoderApp.RunningObject.FileMan.LoadAll(); // do not use file.Load() here as otherwise referenced files won't be loaded
        }

        private void CreateFile()
        {
            var args = OnSelectFile(Res.Status_CreatingFile + " " + Res.File_SelectCreate,
                Environment.CurrentDirectory, false);

            if (args.Cancel)
                return;

            var path = args.Path;

            if (string.IsNullOrWhiteSpace(path))
            {
                OnReport(Res.Status_CreatingFile, string.Format(Res.Error_InvalidFile, path), Interaction.MessageSeverity.Critical);
                return;
            }
            if (ChameleonCoderApp.RunningObject.FileMan.IsOpen(path))
            {
                OnReport(Res.Status_OpeningFile, string.Format(Res.Error_FileAlreadyLoaded, path),
                    Interaction.MessageSeverity.Critical);
                return;
            }
            if (System.IO.File.Exists(path))
            {
                if (OnConfirm(Res.Status_CreatingFile, string.Format(Res.File_ConfirmOverwrite, path)) != true)
                    return;
                else
                    System.IO.File.Delete(path);
            }

            var input_args = OnUserInput(Res.Status_CreatingFile, Res.File_EnterName);
            if (input_args.Cancel)
                return;

            if (string.IsNullOrWhiteSpace(input_args.Input))
                return; // report?

            using (var stream = System.IO.File.Create(path))
            {
                using (var writer = new System.IO.StreamWriter(stream))
                {
                    writer.Write(string.Format(Files.DataFile.fileTemplate, input_args.Input, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")));
                }
            }

            OpenFile(path);
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

        public static string File_Create { get { return Res.File_Create; } }
        public static string File_Open { get { return Res.File_Open; } }
        public static string File_CloseAll { get { return Res.File_CloseAll; } }

        #region RibbonTab headers
        public static string RibbonContext_List { get { return Res.RibbonContext_List; } }
        public static string RibbonContext_Edit { get { return Res.RibbonContext_Edit; } }
        public static string RibbonContext_View { get { return Res.RibbonContext_View; } }
        public static string RibbonContext_Files { get { return Res.RibbonContext_Files; } }
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

        public static string Ref_AddFileRef { get { return Res.Ref_AddFileRef; } }

        public static string Ref_AddDirRef { get { return Res.Ref_AddDirRef; } }

        public static string Ref_Delete { get { return Res.Ref_Delete; } }

        public static string Ref_AddResRef { get { return Res.Ref_AddResRef; } }

        #endregion

        #region events

        internal event EventHandler<Interaction.ViewEventArgs> ViewChanged;

        private void OnViewChanged()
        {
            var handler = ViewChanged;
            if (handler != null)
            {
                handler(this, new Interaction.ViewEventArgs(ActiveTab));
            }
        }

        internal event EventHandler<Interaction.FileSelectionEventArgs> SelectFile;

        private Interaction.FileSelectionEventArgs OnSelectFile(string message, string dir, bool mustExist)
        {
            var handler = SelectFile;

            if (handler != null)
            {
                var args = new Interaction.FileSelectionEventArgs(message, dir, "CC resource files | *.ccr", mustExist);
                handler(this, args);
                return args;
            }

            return null;
        }

        #endregion
    }
}
