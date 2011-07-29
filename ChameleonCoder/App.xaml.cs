using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
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
        internal static MainWindow Gui { get { return Application.Current.MainWindow as MainWindow; } }

        internal static string AppDir { get { return Path.GetDirectoryName(AppPath); } }

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
                    ParseDir(AppDir + "\\Data");
            });

            new MainWindow();
            Gui.breadcrumb.Root = new { Name="Home", children = ResourceManager.GetChildren(),
                Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/home.png")) };

            parallelTask.Wait();
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
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes.AsParallel()
                                            where _ref.Name == "file"
                                            select _ref),
                                      file_ref =>
                                      {
                                          if (File.Exists(file_ref.InnerText))
                                              ParseFile(file_ref.InnerText);
                                          else if (File.Exists(Path.Combine(Path.GetDirectoryName(file), file_ref.InnerText)))
                                              ParseFile(Path.Combine(Path.GetDirectoryName(file), file_ref.InnerText));
                                      }),
                    () => Parallel.ForEach((from XmlNode _ref in doc.DocumentElement.ChildNodes.AsParallel()
                                            where _ref.Name == "dir" && Directory.Exists(_ref.Value)
                                            select _ref),
                                       dir_ref =>
                                       {
                                           if (Directory.Exists(dir_ref.InnerText))
                                               ParseDir(dir_ref.InnerText);
                                           else if (File.Exists(Path.Combine(Path.GetDirectoryName(file), dir_ref.InnerText)))
                                               ParseDir(Path.Combine(Path.GetDirectoryName(file), dir_ref.InnerText));
                                       }));
            }
            else if (!error)
            {
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
                            UnpackResources(file);

                            /*if (!Ionic.Zip.ZipFile.IsZipFile(file))
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
                            }*/


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

        internal static void UnpackResources(string file)
        {
            string packName = FindFreePath(AppDir + "\\Data\\", Path.GetFileNameWithoutExtension(file), false);

            using (Package zip = Package.Open(file, FileMode.Open, FileAccess.Read))
            {
                foreach (PackageRelationship relation in zip.GetRelationshipsByType("ChameleonCoder.Package.Resource").Concat(zip.GetRelationshipsByType("ChameleonCoder.Package.ResourceMap")))
                {
                    Uri source = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relation.TargetUri);
                    PackagePart part = zip.GetPart(source);
                    Uri targetPath = new Uri(new Uri(AppDir + "\\Data\\" + packName, UriKind.Absolute),
                        new Uri(part.Uri.ToString().TrimStart('/'), UriKind.Relative));
                    Directory.CreateDirectory(Path.GetDirectoryName(targetPath.LocalPath));
                    using (FileStream fileStream = new FileStream(targetPath.LocalPath, FileMode.Create))
                    {
                        CopyStream(part.GetStream(), fileStream);
                    }
                }
            }
        }

        static object lock_ = new object();

        static XmlDocument currentMap;
        static bool includeFS;
        static bool includeTarget;

        internal static void PackageResources(IList<IResource> resources)
        {
            lock(lock_)
            {
                if (!Directory.Exists(AppDir + "\\Temp"))
                    Directory.CreateDirectory(AppDir + "\\Temp");

                string tempzip = FindFreePath(AppDir + "\\Temp", "temp_zip", true);

                if (ChameleonCoder.Properties.Settings.Default.UseDefaultPackageSettings)
                {
                    includeFS = ChameleonCoder.Properties.Settings.Default.Package_IncludeFSComponent;
                    includeTarget = ChameleonCoder.Properties.Settings.Default.Package_IncludeTarget;
                }
                else
                {
                    includeFS =
                            MessageBox.Show("Do you want to include the file system equivalents for the packaged resources?",
                                "CC - packaging...",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                    includeTarget =
                            MessageBox.Show("Do you want to include the target resources for the packaged resources?",
                                "CC- packaging...",
                                MessageBoxButton.YesNo) == MessageBoxResult.Yes;
                }

                List<PackagePart> parts = new List<PackagePart>();
                currentMap = new XmlDocument();
                currentMap.LoadXml("<cc-project-map/>");

                using (Package zip = Package.Open(tempzip, FileMode.CreateNew, FileAccess.ReadWrite))
                {
                    foreach (IResource resource in resources)
                        AddPackagePart(zip, resource, parts);
                    foreach (PackagePart part in parts)
                        zip.CreateRelationship(part.Uri, TargetMode.Internal, "ChameleonCoder.Package.Resource");

                    string mapPath = FindFreePath(AppDir + "\\Temp", "package.ccm", true);
                    currentMap.Save(mapPath);
                    PackagePart mapPart = GetPackagePart(zip, mapPath);
                    zip.CreateRelationship(mapPart.Uri, TargetMode.Internal, "ChameleonCoder.Package.ResourceMap");
                    currentMap = null;
                }
                
                File.Move(tempzip, FindFreePath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "pack.zip", true));
                MessageBox.Show("mission complete!");
            }
        }

        private static void AddPackagePart(Package zip, IResource resource, List<PackagePart> collection)
        {
            string path;
            if (resource.Parent == null)
            {
                path = FindFreePath(AppDir + "\\Temp",
                    Path.GetFileNameWithoutExtension(resource.GetResourceFile()) + "." + resource.GUID.ToString("n") + Path.GetExtension(resource.GetResourceFile()), true);
                File.Copy(resource.GetResourceFile(), path);
            }
            else
            {
                path = FindFreePath(AppDir, resource.Name + "." + resource.GUID.ToString("n") + ".ccr", true);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(resource.Xml.OuterXml);
                doc.Save(path);
            }
            PackagePart newPart = GetPackagePart(zip, path);
            if (newPart != null)
                collection.Add(newPart);
            currentMap.DocumentElement.InnerXml += "<file>" + Path.GetFileName(path) + "</file>";

            GetRequiredParts(zip, resource, collection);
        }

        private static PackagePart GetPackagePart(Package zip, string path)
        {
            Uri target = PackUriHelper.CreatePartUri(
                            new Uri(Path.GetFileName(path), UriKind.Relative));
            MessageBox.Show(path + "\n" + Path.GetFileName(path));

            if (!zip.PartExists(target))
            {
                PackagePart part = zip.CreatePart(target, System.Net.Mime.MediaTypeNames.Text.Xml);

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    CopyStream(stream, part.GetStream());
                }
                return part;
            }
            return null;
        }

        private static void GetRequiredParts(Package zip, IResource parent, List<PackagePart> parts)
        {
            if (parent is IFSComponent && includeFS)
                parts.Add(GetPackagePart(zip, (parent as IFSComponent).GetFSPath()));
            if (parent is IResolvable && includeTarget)
                AddPackagePart(zip, (parent as IResolvable).Resolve(), parts);

            foreach (IResource child in parent.children)
                GetRequiredParts(zip, child, parts);
        }

        private static void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }

        private static string FindFreePath(string directory, string baseName, bool isFile)
        {
            directory = directory[directory.Length - 1] == Path.DirectorySeparatorChar
                ? directory : directory + Path.DirectorySeparatorChar;

            baseName = baseName.TrimStart(Path.DirectorySeparatorChar);

            string fileName = isFile
                ? Path.GetFileNameWithoutExtension(baseName) : baseName;

            string Extension = isFile
                ? Path.GetExtension(baseName) : string.Empty;

            string path = directory + fileName + Extension;
            int i = 0;

            while ((isFile ? File.Exists(path) : Directory.Exists(path)))
            {
                path = directory + fileName + "_" + i + Extension;
                i++;
            }

            return path;
        }
    }
}
