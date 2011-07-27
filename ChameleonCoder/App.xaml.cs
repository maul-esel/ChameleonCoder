using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
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
        internal static MainWindow Gui;

        public static string AppDir { get { return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); } }

        public static string AppPath { get { return Assembly.GetEntryAssembly().Location; } }

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
            });

            if (plus_data)
                no_data = false;
            #endregion

            if (!noplugin)
                LoadPlugins();

            App.Current.Exit += ExitHandler;

            Gui = new MainWindow();

            Parallel.Invoke(() => Parallel.ForEach(files, file => ParseFile(file)),
                            () => Parallel.ForEach(dirs, dir => ParseDir(dir)));

            if (!no_data)
                ParseDir(AppDir + "\\Data");
            Gui.breadcrumb.Root = new { Name="Home", children = ResourceManager.GetChildren() };
            Gui.Show();
        }

        internal static void ExitHandler(object sender, EventArgs e)
        {
            LanguageModules.LanguageModuleHost.Shutdown();
            Services.ServiceHost.Shutdown();
        }

        private static void ParseFile(string file)
        {
            bool error = false;

            XmlDocument doc = new XmlDocument();

            try { doc.Load(file); }
            catch (XmlException e) { MessageBox.Show(file + "\n\n" + e.Message); error = true; }

            if (!error && doc.DocumentElement.Name == "cc-project-map")
            {
                Parallel.Invoke(
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes
                                      where _ref.Name == "file" && File.Exists(_ref.Value)
                                      select _ref),
                                      file_ref => ParseFile(file_ref.Value)),
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes
                                       where _ref.Name == "dir" && Directory.Exists(_ref.Value)
                                       select _ref),
                                       dir_ref => ParseDir(dir_ref.Value)));
            }
            else if (!error)
            {
                //MessageBox.Show(file);
                AddResource(doc.DocumentElement, null);
            }
        }

        private static void ParseDir(string dir)
        {
            Parallel.ForEach(
                new ConcurrentBag<string>(
                    (Directory.GetFiles(dir, "*.ccr", SearchOption.AllDirectories)).Concat(
                     Directory.GetFiles(dir, "*.ccm", SearchOption.AllDirectories))),
               file => ParseFile(file));
        }

        internal static bool AddResource(XmlNode node, IResource parent)
        {
            IResource resource;
            
            resource = ResourceTypeManager.CreateInstanceOf(node.Name, node, parent);
            if (resource == null && node.Attributes["fallback"] != null)
                resource = ResourceTypeManager.CreateInstanceOf(node.Attributes["fallback"].Value, node);
            if (resource == null)
            {
                MessageBox.Show(ResourceTypeManager.IsRegistered(node.Name).ToString() + " [ " + node.Name + "]");
                return false;
            }

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

                regMap.SetValue("", "CC resource map");
                regRes.SetValue("", "CC resource file");

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
                            if (!Ionic.Zip.ZipFile.IsZipFile(file))
                                return;

                            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                            {
                                zip.FullScan = true;
                                zip.Initialize(file);

                                Parallel.For(0, zip.Count,
                                    i => {
                                        int index = (int)i;
                                        zip[index].Extract(AppDir + "\\Data", Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);

                                        if (zip[index].IsDirectory)
                                            ParseDir(AppDir + "\\Data" + "\\" + zip[index].FileName);
                                        else
                                            ParseFile(AppDir + "\\Data" + "\\" + zip[index].FileName);
                                        });
                            }

                            return;
                        }

                        App.AddResource(doc.DocumentElement, null);
                        System.IO.File.Copy(file,
                            System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                            + System.IO.Path.DirectorySeparatorChar + "Data" + System.IO.Path.DirectorySeparatorChar
                            + System.IO.Path.GetFileName(file));
                    }
                }
            }
        }
    }
}
