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
        /// gets the directory containing the application
        /// </summary>
        [ComVisible(false)]
        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } } // possibly make COM-visible

        /// <summary>
        /// gets the full path to the application
        /// </summary>
        [ComVisible(false)]
        internal static string AppPath { get { return Assembly.GetEntryAssembly().Location; } } // possibly make COM-visible // todo: will fail in COM

        /// <summary>
        /// gets the initially loaded file
        /// </summary>
        [Obsolete]
        public IDataFile DefaultFile { get; set; }

        /// <summary>
        /// the running System.Windows.Application instance
        /// </summary>
        [ComVisible(false), Obsolete("Attention: null if the instance is created via COM!")]
        internal static Application RunningApp
        {
            get
            {
                return Application.Current;
            }
        }

        /// <summary>
        /// the running ChameleonCoderApp instance
        /// </summary>
        [ComVisible(false), Obsolete("Don't use if avoidable!")]
        public static ChameleonCoderApp RunningObject { get; private set; }

        /// <summary>
        /// the string used to separate resource paths
        /// </summary>
        [ComVisible(false), Obsolete]
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
            ResourceTypeMan = new ResourceTypeManager(this);

            RunningObject = this; // TODO!

            // setting the Language the user chose
            ChameleonCoder.Properties.Resources.Culture = new System.Globalization.CultureInfo(Settings.ChameleonCoderSettings.Default.Language); // setting retrieval fails if invoked from COM

            // associate the instances created in XAML with the classes
            ResourceMan.SetCollections((ResourceCollection)RunningApp.Resources["resources"], (ResourceCollection)RunningApp.Resources["resourceHierarchy"]); // fails if invoked from cOM (App == null)
        }

        /// <summary>
        /// exits the program
        /// <param name="exitCode">the exit code</param>
        /// </summary>
        public void Exit(int exitCode)
        {
            PluginMan.Shutdown(); // inform plugins
            ResourceMan.Shutdown();
            FileMan.Shutdown();

            Environment.Exit(exitCode);
        }

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

        public ResourceTypeManager ResourceTypeMan
        {
            get;
            private set;
        }

        #region file extensions

        /// <summary>
        /// registers the file extension *.ccr
        /// </summary>
        public void RegisterExtension()
        {
            RegistryManager.RegisterExtension();
        }

        /// <summary>
        /// unregisters the file extension *.ccr
        /// </summary>
        public void UnRegisterExtension()
        {
            RegistryManager.UnRegisterExtension();
        }

        #endregion

        #region window management

        /// <summary>
        /// gets the application's main window
        /// </summary>
        public Window Window { get; private set; }

        /// <summary>
        /// creates the main window
        /// </summary>
        /// <exception cref="InvalidOperationException">thrown if the window had already been initialized</exception>
        public void InitWindow()
        {
            if (Window != null)
                throw new InvalidOperationException("Window has already been initialized.");
            Window = (Window)Activator.CreateInstance(Assembly.GetEntryAssembly().GetType("ChameleonCoder.MainWindow"), new object[1] { this }); // TODO! (not elegant, will fail in COM)
        }

        /// <summary>
        /// shows the main window
        /// </summary>
        /// <remarks>If <see cref="InitWindow"/> has not yet been called, it is automatically called before showing thew window.</remarks>
        public void ShowWindow()
        {
            if (Window == null)
                InitWindow();
            Window.Show();
        }

        /// <summary>
        /// hides the main window
        /// </summary>
        /// <exception cref="InvalidOperationException">throw if the window has not yet been initialized</exception>
        public void HideWindow()
        {
            if (Window == null)
                throw new InvalidOperationException("Window has not yet been initialized.");
            Window.Hide();
        }

        #endregion

        #region logging

        /// <summary>
        /// the path to the log file for the app
        /// </summary>
        [ComVisible(false)]
        private static readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChameleonCoder.log");

        /// <summary>
        /// a template for the logging
        /// </summary>
        [ComVisible(false)]
        private const string logTemplate = "new event:"
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
