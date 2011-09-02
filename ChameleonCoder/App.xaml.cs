using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.RichContent;
using Microsoft.Win32;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region properties
        public static System.Windows.Threading.Dispatcher DispatcherObject { get; private set; }

        internal static MainWindow Gui { get { return Application.Current.MainWindow as MainWindow; } }

        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } }

        internal static string AppPath { get { return Assembly.GetEntryAssembly().Location; } }

        internal static DataFile OpenFile { get; private set; }
        #endregion

        internal void Init(Object sender, StartupEventArgs e)
        {
            DispatcherObject = Dispatcher;
            ChameleonCoder.Properties.Resources.Culture = new System.Globalization.CultureInfo(ChameleonCoder.Properties.Settings.Default.Language);

            ResourceTypeManager.SetCollection(Resources["ResourceTypes"] as ResourceTypeCollection);
            ResourceManager.SetCollections(Resources["resources"] as ResourceCollection,
                                           Resources["resourceHierarchy"] as ResourceCollection);

            string path = null;

            if (e.Args.Length > 0)
            {
                if (File.Exists(e.Args[0]))
                {
                    path = e.Args[0];
                }
                else if (e.Args[0] == "--install_ext")
                {
                    if (Registry.ClassesRoot.OpenSubKey(".ccr") != null
                        && Registry.ClassesRoot.OpenSubKey(".ccp") != null)
                        UnRegisterExtensions();
                    else
                        RegisterExtensions();
                    Environment.Exit(0);
                }
                else if (e.Args[0] == "--install_COM")
                {
                    Environment.Exit(-3);
                }
                else if (e.Args[0] == "--install_full")
                {
                    System.Diagnostics.Process.Start(AppPath, "--install_ext");
                    System.Diagnostics.Process.Start(AppPath, "--install_COM");
                    Environment.Exit(0);
                }
            }

            if (path == null)
            {
#if DEBUG
                path = "test.ccr";
#else
                using (var dialog = new System.Windows.Forms.OpenFileDialog() { Filter = "CC Resources|*.ccr; *.ccp" })
                {
                    if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        Environment.Exit(-1);
                    path = dialog.FileName;
                }
#endif
            }

            OpenFile = DataFile.Open(path);

            App.Current.Exit += ExitHandler;

            Task parallelTask = Task.Factory.StartNew(() =>
            {
                if (!e.Args.Contains("--noplugin"))
                    LoadPlugins();
                foreach (XmlElement element in OpenFile.Document.SelectNodes("/cc-resource-file/resources/*"))
                    AddResource(element, null);
            });

            new MainWindow();

            parallelTask.Wait();
            Gui.Show();
        }

        internal static void ExitHandler(object sender, EventArgs e)
        {
            Plugins.PluginManager.Shutdown();
            OpenFile.Save();
            OpenFile.Dispose();
        }

        internal static void AddResource(XmlElement node, IResource parent)
        {
            IResource resource;
            
            resource = ResourceTypeManager.CreateInstanceOf(node.Name);
            if (resource == null && node.Attributes["fallback"] != null)
                resource = ResourceTypeManager.CreateInstanceOf(node.Attributes["fallback"].Value);
            if (resource == null)
                return;

            resource.Init(node, parent);

            IRichContentResource richResource = resource as IRichContentResource;

            ResourceManager.Add(resource, parent);

            foreach (XmlElement child in node.ChildNodes)
            {
                if (child.Name == "RichContent" && richResource != null)
                    foreach (XmlNode member in child.ChildNodes)
                    {
                        IContentMember richContent = ContentMemberManager.CreateInstanceOf(member.Name);
                        if (richContent == null)
                            richContent = ContentMemberManager.CreateInstanceOf(member.Attributes["fallback"].Value);
                        if (richContent != null)
                            if (richResource.ValidateRichContent(richContent))
                                richResource.RichContent.Add(richContent);
                    }
                else
                    AddResource(child, resource);
            }

            return;
        }

        private static void LoadPlugins()
        {
            var components = from dll in Directory.GetFiles(AppDir + "\\Components", "*.dll")
                             let plugin = Assembly.LoadFrom(dll)
                             where Attribute.IsDefined(plugin, typeof(CCPluginAttribute))
                             from type in plugin.GetTypes()
                             where Attribute.IsDefined(type, typeof(CCPluginAttribute))
                                && !type.IsValueType && !type.IsAbstract && type.IsClass && type.IsPublic
                                && type.GetInterface(typeof(Plugins.IPlugin).FullName) != null
                                && type.GetConstructor(Type.EmptyTypes) == null
                             select type;

            Parallel.ForEach(components, component => Plugins.PluginManager.TryAdd(component));
        }

        #region Registry
        static object lock_ext = new object();
 
        internal static void RegisterExtensions()
        {
            lock (lock_ext)
            {
                RegistryKey regCCP = Registry.ClassesRoot.CreateSubKey(".ccp", RegistryKeyPermissionCheck.ReadWriteSubTree);
                RegistryKey regCCR = Registry.ClassesRoot.CreateSubKey(".ccr", RegistryKeyPermissionCheck.ReadWriteSubTree);
                
                regCCP.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCP);
                regCCR.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCR);

                regCCP.Close();
                regCCR.Close();

                regCCP = Registry.ClassesRoot.CreateSubKey(".ccp\\Shell\\Open\\command");
                regCCR = Registry.ClassesRoot.CreateSubKey(".ccr\\Shell\\Open\\command");

                regCCP.SetValue("", "\"" + App.AppPath + "\" \"%1\"");
                regCCR.SetValue("", "\"" + App.AppPath + "\" \"%1\"");

                regCCP.Close();
                regCCR.Close();

                regCCP = Registry.ClassesRoot.CreateSubKey(".ccp\\DefaultIcon");
                regCCR = Registry.ClassesRoot.CreateSubKey(".ccr\\DefaultIcon");

                regCCP.SetValue("", AppPath + ", 0");
                regCCR.SetValue("", AppPath + ", 1");

                regCCP.Close();
                regCCR.Close();
            }
        }

        internal static void UnRegisterExtensions()
        {
            lock (lock_ext)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(".ccp");
                Registry.ClassesRoot.DeleteSubKeyTree(".ccr");
            }
        }
        #endregion

        static object lock_drop = new object();

        [Obsolete]
        internal static void ImportDroppedResource(DragEventArgs dragged)
        {
            lock (lock_drop)
            {
                if (dragged.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] files = dragged.Data.GetData(DataFormats.FileDrop, true) as string[];

                    foreach (string file in files)
                    {
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        try { doc.Load(file); }
                        catch (System.Xml.XmlException)
                        {
                            PackageManager.UnpackResources(file);
                            return;
                        }
                        //File.Copy(file, DataDir + Path.GetFileName(file));
                        //App.ParseFile(DataDir + Path.GetFileName(file));
                    }
                }
            }
        }

        internal static void Log(string sender, string reason, string log)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChameleonCoder.log");
            if (!File.Exists(path))
                File.Create(path);
            File.AppendAllText(path, "new event:"
                + "\n\tsender: " + sender
                + "\n\treason: " + reason
                + "\n\t\t" + log
                + "\n===========================================\n\n");
        }
    }
}
