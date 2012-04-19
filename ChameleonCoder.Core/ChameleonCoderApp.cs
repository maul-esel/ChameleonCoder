using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using ChameleonCoder.Files;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// The COM-visible new main class for the app
    /// </summary>
    [ComVisible(true), ProgId("ChameleonCoder.Application"), Guid("712fc748-468f-45db-ab09-e472b6a97b69"), ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class ChameleonCoderApp
    {
        /// <summary>
        /// gets the application's main window
        /// </summary>
        [ComVisible(false)]
        internal static Window Gui { get; private set; }

        /// <summary>
        /// gets the directory containing the application
        /// </summary>
        [ComVisible(false)]
        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } } // possibly make COM-visible

        /// <summary>
        /// gets the full path to the application
        /// </summary>
        [ComVisible(false)]
        internal static string AppPath { get { return Assembly.GetEntryAssembly().Location; } } // possibly make COM-visible

        /// <summary>
        /// gets the initially loaded file
        /// </summary>
        internal static DataFile DefaultFile { get; set; }

        /// <summary>
        /// the running System.Windows.Application instance
        /// </summary>
        [ComVisible(false), Obsolete("Attention: must check if it is null before usage!")]
        internal static Application RunningApp
        {
            get
            {
                return System.Windows.Application.Current;
            }
            //private set;
        }

        /// <summary>
        /// the running ChameleonCoderApp instance
        /// </summary>
        [ComVisible(false), Obsolete("Don't use if avoidable!")]
        public static ChameleonCoderApp RunningObject { get; private set; }

        /// <summary>
        /// the string used to separate resource paths
        /// </summary>
        [ComVisible(false)]
        internal const string resourcePathSeparator = "/";

        /// <summary>
        /// creates a new instance of the class
        /// </summary>
        /// <remarks>This must be COM-compatible! Do not add parameters!</remarks>
        public ChameleonCoderApp()
        {
            FileMan = new Files.FileManager(this);
            PluginMan = new Plugins.PluginManager(this);
            ResourceMan = new ResourceManager(this);
            ContentMemberMan = new Resources.RichContent.ContentMemberManager(this);

            RunningObject = this; // TODO!

            // setting the Language the user chose
            ChameleonCoder.Properties.Resources.Culture = new System.Globalization.CultureInfo(Settings.ChameleonCoderSettings.Default.Language);

            // associate the instances created in XAML with the classes
            ResourceMan.SetCollections((ResourceCollection)RunningApp.Resources["resources"], (ResourceCollection)RunningApp.Resources["resourceHierarchy"]);
        }

        /// <summary>
        /// exits the program
        /// <param name="exitCode">the exit code</param>
        /// </summary>
        [DispId(1)]
        public void Exit(int exitCode)
        {
            PluginMan.Shutdown(); // inform plugins
            FileMan.SaveAll(); // save changes to the opened files
            FileMan.CloseAll();

            Environment.Exit(exitCode);
        }

        [DispId(2)]
        public FileManager FileMan
        {
            get;
            private set;
        }

        public Plugins.PluginManager PluginMan
        {
            get;
            private set;
        }

        public ResourceManager ResourceMan
        {
            get;
            private set;
        }

        public Resources.RichContent.ContentMemberManager ContentMemberMan
        {
            get;
            private set;
        }

        #region file extensions

        /// <summary>
        /// registers the file extension *.ccr
        /// </summary>
        [DispId(3)]
        internal void RegisterExtension()
        {
            RegistryManager.RegisterExtension();
        }

        /// <summary>
        /// unregisters the file extension *.ccr
        /// </summary>
        [DispId(4)]
        internal void UnRegisterExtension()
        {
            RegistryManager.UnRegisterExtension();
        }

        #endregion

        /// <summary>
        /// loads all the plugins
        /// </summary>
        [DispId(5)]
        public void LoadPlugins()
        {
            PluginMan.Load();
        }

        #region window management

        /// <summary>
        /// creates the main window
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if the window had already been initialized</exception>
        [DispId(6)]
        public void InitWindow()
        {
            if (Gui != null)
                throw new InvalidOperationException("Window has already been initialized.");
            Gui = new Window(); // TODO!
        }

        /// <summary>
        /// shows the main window
        /// </summary>
        /// <remarks>If <see cref="InitWindow"/> has not yet been called, it is called before showing thew window.</remarks>
        [DispId(7)]
        public void Show()
        {
            if (Gui == null)
                this.InitWindow();
            Gui.Show();
        }

        /// <summary>
        /// hides the main window
        /// </summary>
        /// <exception cref="InvalidOperationException">throw if the window has not yet been initialized</exception>
        [DispId(8)]
        public void Hide()
        {
            if (Gui == null)
                throw new InvalidOperationException("Window has not yet been initialized.");
            Gui.Hide();
        }

        #endregion

        #region logging

        /// <summary>
        /// the path to the log file for the app
        /// </summary>
        [ComVisible(false)]
        internal static readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChameleonCoder.log");

        /// <summary>
        /// a template for the logging
        /// </summary>
        [ComVisible(false)]
        internal const string logTemplate = "new event:"
                                          + "\n\tsender: {0}"
                                          + "\n\treason: {1}"
                                          + "\n\t\t{2}"
                                          + "\n===========================================\n\n";

        /// <summary>
        /// logs a message, such as a warning, an error, ...
        /// </summary>
        /// <param name="sender">the sender calling this method</param>
        /// <param name="reason">the reason for the logging</param>
        /// <param name="text">more information about the event</param>
        [ComVisible(false)]
        public static void Log(string sender, string reason, string text)
        {
            if (!File.Exists(logPath)) // create file if necessary
                File.Create(logPath).Close(); // (and immediately close the stream)
            File.AppendAllText(logPath, string.Format(logTemplate, sender, reason, text)); // log
        }

        #endregion
    }
}
