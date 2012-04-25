using System.Windows.Input;

namespace ChameleonCoder
{
    /// <summary>
    /// a class containing RoutedCommands special to ChameleonCoder
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public static class ChameleonCoderCommands
    {
        #region page management
        /// <summary>
        /// the command for opening the page with a list of resources
        /// </summary>
        public static readonly RoutedCommand OpenResourceListPage
            = new RoutedCommand("OpenResourceListPage",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for opening the page managing plugins
        /// </summary>
        public static readonly RoutedCommand OpenPluginPage
            = new RoutedCommand("OpenPluginPage",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for opening the page managing settings
        /// </summary>
        public static readonly RoutedCommand OpenSettingsPage
            = new RoutedCommand("OpenSettingsPage",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for opening a specific resource
        /// </summary>
        public static readonly RoutedCommand OpenResourceView
            = new RoutedCommand("OpenResourceView",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for opening a specific resource in edit view
        /// </summary>
        public static readonly RoutedCommand OpenResourceEdit
            = new RoutedCommand("OpenResourceEdit",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for opening a DataFile management page
        /// </summary>
        public static readonly RoutedCommand OpenFileManagementPage
            = new RoutedCommand("OpenFileManagementPage",
                                typeof(ChameleonCoderCommands));
        #endregion

        #region resources
        /// <summary>
        /// the command for creating a new resource
        /// </summary>
        public static readonly RoutedCommand CreateResource
            = new RoutedCommand("CreateResource",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for saving a resource
        /// </summary>
        public static readonly RoutedCommand SaveResource
            = new RoutedCommand("SaveResource",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for deleting a resource
        /// </summary>
        public static readonly RoutedCommand DeleteResource
            = new RoutedCommand("SaveResource",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for adding a resource to a collection
        /// </summary>
        public static readonly RoutedCommand AddResource
            = new RoutedCommand("AddResource",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for removing a resource from a collection
        /// </summary>
        public static readonly RoutedCommand RemoveResource
            = new RoutedCommand("RemoveResource",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for copying a resource
        /// </summary>
        public static readonly RoutedCommand CopyResource
            = new RoutedCommand("CopyResource",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for moving a resource
        /// </summary>
        public static readonly RoutedCommand MoveResource
            = new RoutedCommand("MoveResource",
                                typeof(ChameleonCoderCommands));
        #endregion

        #region tabs
        /// <summary>
        /// the command for opening a new tab
        /// </summary>
        public static readonly RoutedCommand OpenNewTab
            = new RoutedCommand("OpenNewTab",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for closing a tab
        /// </summary>
        public static readonly RoutedCommand CloseTab
            = new RoutedCommand("CloseTab",
                                typeof(ChameleonCoderCommands));
        #endregion

        /// <summary>
        /// the command for executing a service plugin
        /// </summary>
        public static readonly RoutedCommand ExecuteService
            = new RoutedCommand("ExecuteService",
                                typeof(ChameleonCoderCommands));

        #region metadata
        /// <summary>
        /// the command for adding metadata on a resource or DataFile
        /// </summary>
        public static readonly RoutedCommand AddMetadata
            = new RoutedCommand("AddMetadata",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for deleting metadata on a resource or DataFile
        /// </summary>
        public static readonly RoutedCommand DeleteMetadata
            = new RoutedCommand("DeleteMetadata",
                                typeof(ChameleonCoderCommands));
        #endregion

        /// <summary>
        /// the command for setting the sorting mode in a list box / -view
        /// </summary>
        public static readonly RoutedCommand SetSortingMode
            = new RoutedCommand("SetSortingMode",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for setting the grouping mode in a list box / -view
        /// </summary>
        public static readonly RoutedCommand SetGroupingMode
            = new RoutedCommand("SetGroupingMode",
                                typeof(ChameleonCoderCommands));

        #region plugins
        /// <summary>
        /// the command for uninstalling a plugin
        /// </summary>
        public static readonly RoutedCommand UninstallPlugin
            = new RoutedCommand("UninstallPlugin",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for installing plugins
        /// </summary>
        public static readonly RoutedCommand InstallPlugin
            = new RoutedCommand("InstallPlugin",
                                typeof(ChameleonCoderCommands));
        #endregion

        #region files
        /// <summary>
        /// the command for opening a new file, adding it to the already opened ones
        /// </summary>
        public static readonly RoutedCommand OpenFile
            = new RoutedCommand("OpenFile",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for closing all opened files
        /// </summary>
        public static readonly RoutedCommand CloseFiles
            = new RoutedCommand("CloseFiles",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for creating a new file, opening it afterwards
        /// </summary>
        public static readonly RoutedCommand CreateFile
            = new RoutedCommand("CreateFile",
                                typeof(ChameleonCoderCommands));
        #endregion

        #region resource references
        /// <summary>
        /// the command for creating a new reference on a resource
        /// </summary>
        public static readonly RoutedCommand AddReference
            = new RoutedCommand("AddReference",
                                typeof(ChameleonCoderCommands));

        /// <summary>
        /// the command for deleting a reference on a resource
        /// </summary>
        public static readonly RoutedCommand DeleteReference
            = new RoutedCommand("DeleteReference",
                                typeof(ChameleonCoderCommands));

        #endregion

        #region DataFile references

        public static readonly RoutedCommand AddFileReference
            = new RoutedCommand("AddFileReference",
                                typeof(ChameleonCoderCommands));

        public static readonly RoutedCommand DeleteFileReference
            = new RoutedCommand("DeleteFileReference",
                                typeof(ChameleonCoderCommands));

        public static readonly RoutedCommand AddDirectoryReference
            = new RoutedCommand("AddDirectoryReference",
                                typeof(ChameleonCoderCommands));

        public static readonly RoutedCommand DeleteDirectoryReference
            = new RoutedCommand("DeleteDirectoryReference",
                                typeof(ChameleonCoderCommands));

        #endregion
    }
}
