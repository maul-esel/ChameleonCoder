using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using Microsoft.Win32;

namespace ChameleonCoder
{
    /// <summary>
    /// the main app class
    /// </summary>
    public partial class App : Application
    {
        internal const string pathSeparator = "/";

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
            // finding the path of the file to open:
            string path = null;

            // parsing command line: 
            if (e.Args.Length > 0)
            {
                if (File.Exists(e.Args[0])) // if a file path is passed: use it as path
                {
                    path = e.Args[0];
                }
                else if (e.Args[0].Equals("--install_ext", StringComparison.OrdinalIgnoreCase)) // param to install file extension
                {
                    if (Registry.ClassesRoot.OpenSubKey(".ccr") == null)
                        RegisterExtension();
                    Environment.Exit(0); // shutdown the app
                }
                else if (e.Args[0].Equals("--uninstall_ext", StringComparison.OrdinalIgnoreCase)) // param to uninstall file extension
                {
                    if (Registry.ClassesRoot.OpenSubKey(".ccr") != null)
                        UnRegisterExtension();
                    Environment.Exit(0); // shutdown the app
                }
                else if (e.Args[0].Equals("--install_COM", StringComparison.OrdinalIgnoreCase)) // param to install COM support
                {
                    Environment.Exit(-3); // not yet implemented
                }
                else if (e.Args[0].Equals("--uninstall_COM", StringComparison.OrdinalIgnoreCase)) // param to uninstall COM support
                {
                    Environment.Exit(-3); // not yet implemented
                }
                else if (e.Args[0].Equals("--install_full", StringComparison.OrdinalIgnoreCase)) // param to install file extensions and COM support
                {
                    System.Diagnostics.Process.Start(AppPath, "--install_ext");
                    System.Diagnostics.Process.Start(AppPath, "--install_COM");
                    Environment.Exit(0);
                }
                else if (e.Args[0].Equals("--uninstall_full", StringComparison.OrdinalIgnoreCase)) // param to uninstall file extensions and COM support
                {
                    System.Diagnostics.Process.Start(AppPath, "--uninstall_ext");
                    System.Diagnostics.Process.Start(AppPath, "--uninstall_COM");
                    Environment.Exit(0);
                }
            }

            // setting the Language the user chose
            ChameleonCoder.Properties.Resources.Culture = new System.Globalization.CultureInfo(Settings.ChameleonCoderSettings.Default.Language);

            // associate the instances created in XAML with the classes
            ResourceManager.SetCollections(Resources["resources"] as ResourceCollection,
                                           Resources["resourceHierarchy"] as ResourceCollection);

#if DEBUG
            /* TODO:
             * - do not force the user to open a file
             * - let him open an empty app
             * - implement possibilities to create & open files later
             * - implement possibility to close all opened files
             * - set debug file through project properties >> parameters instead
             */
            if (path == null && File.Exists("test.ccr")) // if no file passed:
                path = "test.ccr"; // use test file in debug builds
#endif
            if (path == null) // if no path was passed:
            {
                // let the user open a new file
                using (var dialog = new System.Windows.Forms.OpenFileDialog() { Filter = "CC Resources|*.ccr" })
                {
                    if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        Environment.Exit(-1);
                    path = dialog.FileName;
                }
            }

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
            IResource resource;
            
            resource = ResourceTypeManager.CreateInstanceOf(node.Name, node, parent); // try to use the element's name as resource alias
            if (resource == null && node.GetAttribute("fallback") != null) // if e.g. the containing plugin is not loaded:
                resource = ResourceTypeManager.CreateInstanceOf(node.GetAttribute("fallback"), node, parent); // give it a "2nd chance"
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
        internal static void RegisterExtension()
        {
            RegistryKey regCCR = Registry.ClassesRoot.CreateSubKey(".ccr", RegistryKeyPermissionCheck.ReadWriteSubTree);
            regCCR.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCR);
            regCCR.Close();

            regCCR = Registry.ClassesRoot.CreateSubKey(".ccr\\Shell\\Open\\command");
            regCCR.SetValue("", "\"" + App.AppPath + "\" \"%1\"");
            regCCR.Close();

            regCCR = Registry.ClassesRoot.CreateSubKey(".ccr\\DefaultIcon");
            regCCR.SetValue("", AppPath + ", 1");
            regCCR.Close();
        }

        /// <summary>
        /// unregisters the file extensions *.ccr
        /// </summary>
        internal static void UnRegisterExtension()
        {
            Registry.ClassesRoot.DeleteSubKeyTree(".ccr");
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
