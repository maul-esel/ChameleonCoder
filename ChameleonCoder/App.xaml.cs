using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// the main app class
    /// </summary>
    public partial class App : Application
    {
        // this field is for temporary use only, it will be removed / changed later
        internal static readonly ChameleonCoderApp instance = new ChameleonCoderApp();

        internal const string pathSeparator = "/";

        internal const string fileTemplate = @"<cc:ChameleonCoder xmlns:cc='ChameleonCoder://Resources/Schema/2011'>"
                                                + "<cc:resources/>"
                                                + "<cc:data/>"
                                                + "<cc:settings>"
                                                    + "<cc:name>{0}</cc:name>"
                                                    + "<cc:created>{1}</cc:created>"
                                                + "</cc:settings>"
                                                + "<cc:references/>"
                                            + "</cc:ChameleonCoder>";

        #region parameter constants

        /// <summary>
        /// param to install file extension
        /// </summary>
        private const string paramInstallExt = "--install_ext";

        /// <summary>
        /// param to uninstall file extension
        /// </summary>
        private const string paramUnInstallExt = "--uninstall_ext";

        /// <summary>
        /// param to install COM support
        /// </summary>
        private const string paramInstallCOM = "--install_COM";

        /// <summary>
        /// param to uninstall COM support
        /// </summary>
        private const string paramUnInstallCOM = "--uninstall_COM";

        #endregion

        #region properties

        /// <summary>
        /// gets the application's main window
        /// </summary>
        internal static MainWindow Gui { get { return Application.Current.MainWindow as MainWindow; } }

        /// <summary>
        /// gets the directory containing the application
        /// </summary>
        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } }

        /// <summary>
        /// gets the full path to the application
        /// </summary>
        internal static string AppPath { get { return Assembly.GetEntryAssembly().Location; } }

        /// <summary>
        /// gets the initially loaded file
        /// </summary>
        internal static DataFile DefaultFile { get; private set; }

        #endregion

        /// <summary>
        /// serves as entry point for the application, reacting to the App.Startup event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data containing the cmd arguments</param>
        private void InitHandler(object sender, StartupEventArgs e)
        {
            string path = null;
            int exitCode = 0;

            // parsing command line:
            int argIndex = 0;
            foreach (string arg in e.Args)
            {
                if (argIndex == e.Args.Length - 1 && File.Exists(arg))
                    path = arg;
                else
                {
                    if (arg.Equals(paramInstallExt, StringComparison.OrdinalIgnoreCase))
                    {
                        //if (Registry.ClassesRoot.OpenSubKey(".ccr") == null) // this step is done in the RegistryManager class now
                            RegisterExtension();
                        // Environment.Exit(0); // shutdown the app (this is now done below the if)
                    }
                    else if (arg.Equals(paramUnInstallExt, StringComparison.OrdinalIgnoreCase))
                    {
                        //if (Registry.ClassesRoot.OpenSubKey(".ccr") != null) // this step is done in the RegistryManager class now
                            UnRegisterExtension();
                        // Environment.Exit(0); // shutdown the app (this is now done below the if)
                    }
                    else if (arg.Equals(paramInstallCOM, StringComparison.OrdinalIgnoreCase))
                    {
                        //Environment.Exit(-3); // not yet implemented
                        exitCode = -3;
                    }
                    else if (arg.Equals(paramUnInstallCOM, StringComparison.OrdinalIgnoreCase))
                    {
                        //Environment.Exit(-3); // not yet implemented
                        exitCode = -3;
                    }
                }
                argIndex++;
            }
            if (path == null) // no file to open was specified
                Environment.Exit(exitCode);

            // setting the Language the user chose
            ChameleonCoder.Properties.Resources.Culture = new System.Globalization.CultureInfo(Settings.ChameleonCoderSettings.Default.Language);

            // associate the instances created in XAML with the classes
            ResourceManager.SetCollections(Resources["resources"] as ResourceCollection,
                                           Resources["resourceHierarchy"] as ResourceCollection);

            // use a second task to speed things up
            Task parallelTask = Task.Factory.StartNew(() =>
            {
                Task files = null;
                if (path != null)
                {
                    files = Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                DefaultFile = DataFile.Open(path); // open the file
                            }
                            catch (FileFormatException)
                            {
                                throw; // Todo: inform user, log
                            }
                            catch (FileNotFoundException)
                            {
                                throw; // Todo: inform user, log
                            }
                        });
                }
                var plugins = Task.Factory.StartNew(() =>
                    {
                        Plugins.PluginManager.Load();// load all plugins in the /Component/ folder
                    });

                if (files != null)
                    files.Wait();

                plugins.Wait();

                foreach (var file in DataFile.LoadedFiles)
                {
                    foreach (XmlElement element in file.GetResources())
                        AddResource(element, null); // and parse the Xml
                }
            });

            MainWindow = new MainWindow(); // open the main window during plugin loading and parsing

            parallelTask.Wait(); // ensure parsing is finished before...
            Gui.Show(); // ... showing the window
        }

        /// <summary>
        /// performs necessary clean-ups on exit
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ExitHandler(object sender, EventArgs e)
        {
            Plugins.PluginManager.Shutdown(); // inform plugins
            DataFile.SaveAll(); // save changes to the opened files
            DataFile.CloseAll();
        }

        /// <summary>
        /// parses a XmlElement and its child elements for resource definitions
        /// and creates instances for them, adding them to the global resource list
        /// and to the given parent resource.
        /// </summary>
        /// <param name="node">the XmlElement to parse</param>
        /// <param name="parent">the parent resource or null,
        /// if the resource represented by <paramref name="node"/> is a top-level resource.</param>
        internal static void AddResource(XmlElement node, IResource parent)
        {
            Guid type;
            IResource resource = null;

            if (Guid.TryParse(node.GetAttribute("type", DataFile.NamespaceUri), out type))
            {
                resource = ResourceTypeManager.CreateInstanceOf(type, node, parent); // try to use the element's name as resource alias
            }
            else if (Guid.TryParse(node.GetAttribute("fallback", DataFile.NamespaceUri), out type))
            {
                resource = ResourceTypeManager.CreateInstanceOf(type, node, parent); // give it a "2nd chance"                    
            }

            if (resource == null) // if creation failed:
            {
                Log("ChameleonCoder.App --> internal static void AddResource(XmlElement, IResource)",
                    "failed to create resource",
                    "resource-creation failed on:\n\t" +
                     node.OuterXml + " in " + DataFile.GetResourceFile(node.OwnerDocument).FilePath); // log
                return; // ignore
            }

            ResourceManager.Add(resource, parent); // and add it to all required lists

            foreach (XmlElement child in node.ChildNodes)
            {
                AddResource(child, resource); // parse all child resources
            }
            resource.LoadReferences();

            // convert it into a RichContentResource
            IRichContentResource richResource = resource as IRichContentResource;
            if (richResource != null) // if it is really a RichContentResource:
            {
                richResource.MakeRichContent(); // parse the RichContent
            }

            return;
        }

        #region Registry
        /// <summary>
        /// registers the file extensions *.ccr
        /// </summary>
        [Obsolete("use ChameleonCoderApp.RegisterExtension() instead.")]
        internal static void RegisterExtension()
        {
            instance.RegisterExtension();
        }

        /// <summary>
        /// unregisters the file extensions *.ccr
        /// </summary>
         [Obsolete("use ChameleonCoderApp.UnRegisterExtension() instead.")]
        internal static void UnRegisterExtension()
        {
            instance.UnRegisterExtension();
        }
        #endregion

        /// <summary>
        /// logs a message, such as a warning, an error, ...
        /// </summary>
        /// <param name="sender">the sender calling this method</param>
        /// <param name="reason">the reason for the logging</param>
        /// <param name="text">more information about the event</param>
        public static void Log(string sender, string reason, string text)
        {
            // get the log path
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChameleonCoder.log");
            if (!File.Exists(path)) // create it if necessary
                File.Create(path).Close(); // (and immediately close the stream)
            File.AppendAllText(path, "new event:" // add the log to the file
                + "\n\tsender: " + sender
                + "\n\treason: " + reason
                + "\n\t\t" + text
                + "\n===========================================\n\n");
        }
    }
}
