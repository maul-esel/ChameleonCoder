using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
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
        public static readonly System.Windows.Threading.Dispatcher DispatcherObject = System.Windows.Threading.Dispatcher.CurrentDispatcher;

        internal static MainWindow Gui { get { return Application.Current.MainWindow as MainWindow; } }

        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } }

        internal static string DataDir { get { return AppDir + "\\Data"; } }

        internal static string AppPath { get { return Assembly.GetEntryAssembly().Location; } }

        internal void Init(Object sender, StartupEventArgs e)
        {
            ResourceTypeManager.SetCollection(this.Resources["ResourceTypes"] as ResourceTypeCollection);
            ResourceManager.SetCollections(this.Resources["resources"] as ResourceCollection,
                                           this.Resources["resourceHierarchy"] as ResourceCollection);

            #region command line
            bool no_data = false;
            bool plus_data = false;
            bool noplugin = false;

            ConcurrentBag<string> files = new ConcurrentBag<string>();
            ConcurrentBag<string> dirs = new ConcurrentBag<string>();

            string[] args = Environment.GetCommandLineArgs();

            Parallel.For(1, args.Length, i =>
            {
                if (File.Exists(args[i]))
                {
                    no_data = true;
                    files.Add(args[i]);
                }
                else if (Directory.Exists(args[i]))
                {
                    no_data = true;
                    dirs.Add(args[i]);
                }
                else if (args[i] == "--data")
                {
                    plus_data = true;
                }
                else if (args[i] == "--install_ext")
                {
                    if (Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccm") != null
                    && Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(".ccr") != null)
                        UnRegisterExtensions();
                    else
                        RegisterExtensions();
                    App.Current.Shutdown();
                }
                else if (args[i] == "--install_COM")
                {
                    App.Current.Shutdown();
                }
                else if (args[i] == "--install_full")
                {
                    System.Diagnostics.Process.Start(AppPath, "--install_ext");
                    System.Diagnostics.Process.Start(AppPath, "--install_COM");
                    App.Current.Shutdown();
                }
            });

            if (plus_data)
                no_data = false;
            #endregion

            App.Current.Exit += ExitHandler;

            Task parallelTask = Task.Factory.StartNew(() =>
            {
                if (!noplugin)
                    LoadPlugins();
                Parallel.Invoke(() => Parallel.ForEach(files, file => ParseFile(file)),
                                () => Parallel.ForEach(dirs, dir => ParseDir(dir)));
                if (!no_data)
                    ParseDir(DataDir);
            });

            new MainWindow();
            Gui.breadcrumb.Root = new BreadcrumbContext("Home",
                new BitmapImage(new Uri("pack://application:,,,/Images/home.png")),
                new BreadcrumbContext[]
                    {
                    new BreadcrumbContext(ChameleonCoder.Properties.Resources.Item_List,
                        new BitmapImage(new Uri("pack://application:,,,/Images/list.png")),
                        ResourceManager.GetChildren(),
                        true, false),
                    new BreadcrumbContext(ChameleonCoder.Properties.Resources.Item_Settings,
                        new BitmapImage(new Uri("pack://application:,,,/Images/RibbonTab1/settings.png")),
                        null,
                        false, true)
                    });

            parallelTask.Wait();
            Gui.Show();
        }

        internal static void ExitHandler(object sender, EventArgs e)
        {
            LanguageModules.LanguageModuleHost.Shutdown();
            Services.ServiceHost.Shutdown();
        }

        internal static void ParseFile(string file)
        {
            bool error = false;

            XmlDocument doc = new XmlDocument();

            try { doc.Load(file); }
            catch (XmlException e) { MessageBox.Show(file + "\n\n" + e.Message); error = true; }

            if (!error && doc.DocumentElement.Name == "cc-project-map")
            {
                Parallel.Invoke(
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes.AsParallel()
                                            where _ref.Name == "file"
                                            select _ref),
                                      file_ref =>
                                      {
                                          if (File.Exists(file_ref.InnerText))
                                              ParseFile(file_ref.InnerText);
                                          else if (File.Exists(Path.Combine(Path.GetDirectoryName(file), file_ref.InnerText)))
                                              ParseFile(Path.GetDirectoryName(file) + "\\" + file_ref.InnerText);
                                          else
                                              MessageBox.Show(Path.Combine(Path.GetDirectoryName(file) + "\\", file_ref.InnerText));
                                      }),
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes.AsParallel()
                                            where _ref.Name == "dir" && Directory.Exists(_ref.Value)
                                            select _ref),
                                       dir_ref =>
                                       {
                                           if (Directory.Exists(dir_ref.InnerText))
                                               ParseDir(dir_ref.InnerText);
                                           else if (File.Exists(Path.GetDirectoryName(file) + "\\" + dir_ref.InnerText))
                                               ParseDir(Path.GetDirectoryName(file) + "\\" + dir_ref.InnerText);
                                       }));
            }
            else if (!error)
            {
                AddResource(doc.DocumentElement, null);
            }
        }

        internal static void ParseDir(string dir)
        {
            Parallel.ForEach(
                new ConcurrentBag<string>(
                    (Directory.GetFiles(dir, "*.ccr", SearchOption.AllDirectories)).Concat(
                     Directory.GetFiles(dir, "*.ccm", SearchOption.AllDirectories))),
               file => ParseFile(file));
        }

        private static bool AddResource(XmlNode node, IResource parent)
        {
            IResource resource;
            
            resource = ResourceTypeManager.CreateInstanceOf(node.Name);
            if (resource == null && node.Attributes["fallback"] != null)
                resource = ResourceTypeManager.CreateInstanceOf(node.Attributes["fallback"].Value);
            if (resource == null)
            {
                MessageBox.Show(ResourceTypeManager.IsRegistered(node.Name).ToString() + " [ " + node.Name + "]");
                return false;
            }
            resource.Init(node, parent);

            IRichContentResource richResource = resource as IRichContentResource;

            ResourceManager.Add(resource, parent);

            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == "metadata")
                    continue;
                else if (child.Name == "RichContent" && richResource != null)
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

            return true;
        }

        private static void LoadPlugins()
        {
            ConcurrentBag<Type> components = new ConcurrentBag<Type>();
            Parallel.ForEach(Directory.GetFiles(AppDir + "\\Components", "*.dll"), dll =>
                Parallel.ForEach(Assembly.LoadFrom(dll).GetTypes(), type => components.Add(type)));

            Parallel.ForEach(components, component =>
            {
                if (!component.IsAbstract && !component.IsInterface && !component.IsNotPublic)
                {
                    if (component.GetInterface(typeof(IComponentProvider).FullName) != null)
                        (Activator.CreateInstance(component) as IComponentProvider).Init(ContentMemberManager.RegisterComponent, ResourceTypeManager.RegisterComponent);

                    if (component.GetInterface(typeof(LanguageModules.ILanguageModule).FullName) != null)
                        LanguageModules.LanguageModuleHost.Add(component);

                    if (component.GetInterface(typeof(Services.IService).FullName) != null)
                        Services.ServiceHost.Add(component);
                }
            });
        }

        static object lock_ext = new object();
 
        internal static void RegisterExtensions()
        {
            lock (lock_ext)
            {
                RegistryKey regMap = Registry.ClassesRoot.CreateSubKey(".ccm", RegistryKeyPermissionCheck.ReadWriteSubTree);
                RegistryKey regRes = Registry.ClassesRoot.CreateSubKey(".ccr", RegistryKeyPermissionCheck.ReadWriteSubTree);

                regMap.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCM);
                regRes.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCR);

                regMap.Close();
                regRes.Close();

                regMap = Registry.ClassesRoot.CreateSubKey(".ccm\\Shell\\Open\\command");
                regRes = Registry.ClassesRoot.CreateSubKey(".ccr\\Shell\\Open\\command");

                regMap.SetValue("", "\"" + App.AppPath + "\" \"%1\"");
                regRes.SetValue("", "\"" + App.AppPath + "\" \"%1\"");

                regMap.Close();
                regRes.Close();

                regMap = Registry.ClassesRoot.CreateSubKey(".ccm\\DefaultIcon");
                regRes = Registry.ClassesRoot.CreateSubKey(".ccr\\DefaultIcon");

                regMap.SetValue("", App.AppPath + ", 0");
                regRes.SetValue("", App.AppPath + ", 1");

                regMap.Close();
                regRes.Close();
            }
        }

        internal static void UnRegisterExtensions()
        {
            lock (lock_ext)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(".ccm");
                Registry.ClassesRoot.DeleteSubKeyTree(".ccr");
            }
        }

        static object lock_drop = new object();

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
                        File.Copy(file, DataDir + Path.GetFileName(file));
                        App.ParseFile(DataDir + Path.GetFileName(file));
                    }
                }
            }
        }
    }
}
