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
        #region properties
        public static System.Windows.Threading.Dispatcher DispatcherObject { get; private set; }

        internal static MainWindow Gui { get { return Application.Current.MainWindow as MainWindow; } }

        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } }

        internal static string DataDir { get { return AppDir + "\\Data"; } }

        internal static string AppPath { get { return Assembly.GetEntryAssembly().Location; } }
        #endregion

        internal void Init(Object sender, StartupEventArgs e)
        {
            DispatcherObject = this.Dispatcher;
            ChameleonCoder.Properties.Resources.Culture = new System.Globalization.CultureInfo(ChameleonCoder.Properties.Settings.Default.Language);

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
                    if (Registry.ClassesRoot.OpenSubKey(".ccm") != null
                        && Registry.ClassesRoot.OpenSubKey(".ccr") != null
                        && Registry.ClassesRoot.OpenSubKey(".ccp") != null)
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
            Gui.breadcrumb.Root = new BreadcrumbContext(ChameleonCoder.Properties.Resources.Item_Home,
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
            XmlDocument doc = new XmlDocument();

            try { doc.Load(file); }
            catch (XmlException)
            {
                PackageManager.UnpackResources(file);
                return;
            }

            if (doc.DocumentElement.Name == "cc-resource-map")
            {
                Parallel.Invoke(
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes.AsParallel()
                                            where _ref.Name == "file"
                                            select _ref),
                                      file_ref =>
                                      {
                                          if (File.Exists(file_ref.InnerText))
                                              ParseFile(file_ref.InnerText);
                                          else if (File.Exists(Path.Combine(Path.GetDirectoryName(file), file_ref.InnerText.TrimStart('\\'))))
                                              ParseFile(Path.GetDirectoryName(file) + "\\" + file_ref.InnerText.TrimStart('\\'));
                                      }),
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes.AsParallel()
                                            where _ref.Name == "dir" && Directory.Exists(_ref.Value)
                                            select _ref),
                                       dir_ref =>
                                       {
                                           if (Directory.Exists(dir_ref.InnerText))
                                               ParseDir(dir_ref.InnerText);
                                           else if (File.Exists(Path.GetDirectoryName(file) + "\\" + dir_ref.InnerText.TrimStart('\\')))
                                               ParseDir(Path.GetDirectoryName(file) + "\\" + dir_ref.InnerText.TrimStart('\\'));
                                       }));
            }
            else
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

        private static void AddResource(XmlNode node, IResource parent)
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

            return;
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
                RegistryKey regCCM = Registry.ClassesRoot.CreateSubKey(".ccm", RegistryKeyPermissionCheck.ReadWriteSubTree);
                RegistryKey regCCP = Registry.ClassesRoot.CreateSubKey(".ccp", RegistryKeyPermissionCheck.ReadWriteSubTree);
                RegistryKey regCCR = Registry.ClassesRoot.CreateSubKey(".ccr", RegistryKeyPermissionCheck.ReadWriteSubTree);
                
                regCCM.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCM);
                regCCP.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCP);
                regCCR.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCR);

                regCCM.Close();
                regCCP.Close();
                regCCR.Close();

                regCCM = Registry.ClassesRoot.CreateSubKey(".ccm\\Shell\\Open\\command");
                regCCP = Registry.ClassesRoot.CreateSubKey(".ccp\\Shell\\Open\\command");
                regCCR = Registry.ClassesRoot.CreateSubKey(".ccr\\Shell\\Open\\command");

                regCCM.SetValue("", "\"" + App.AppPath + "\" \"%1\"");
                regCCP.SetValue("", "\"" + App.AppPath + "\" \"%1\"");
                regCCR.SetValue("", "\"" + App.AppPath + "\" \"%1\"");

                regCCM.Close();
                regCCP.Close();
                regCCR.Close();

                regCCM = Registry.ClassesRoot.CreateSubKey(".ccm\\DefaultIcon");
                regCCP = Registry.ClassesRoot.CreateSubKey(".ccp\\DefaultIcon");
                regCCR = Registry.ClassesRoot.CreateSubKey(".ccr\\DefaultIcon");

                regCCM.SetValue("", AppPath + ", 0");
                regCCP.SetValue("", AppPath + ", 1");
                regCCR.SetValue("", AppPath + ", 2");

                regCCM.Close();
                regCCP.Close();
                regCCR.Close();
            }
        }

        internal static void UnRegisterExtensions()
        {
            lock (lock_ext)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(".ccm");
                Registry.ClassesRoot.DeleteSubKeyTree(".ccp");
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
