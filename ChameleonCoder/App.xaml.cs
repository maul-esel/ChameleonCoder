using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static MainWindow Gui;

        public static string AppDir { get { return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location); } }

        internal void Init(Object sender, StartupEventArgs e)
        {
            ResourceTypeManager.SetCollection(this.Resources["ResourceTypes"] as ResourceTypeCollection);
            ResourceManager.SetCollections(this.Resources["resources"] as ResourceCollection,
                                           this.Resources["resourceHierarchy"] as ResourceCollection);

            #region command line
            bool no_data = false;
            bool plus_data = false;
            bool noplugin = false;

            string[] args = Environment.GetCommandLineArgs();

            for (int i = 1; i < args.Length; i++)
            {
                if (File.Exists(args[i]))
                {
                    no_data = true;
                    ParseFile(args[i]);
                }
                else if (Directory.Exists(args[i]))
                {
                    no_data = true;
                    ParseDir(args[i]);
                }
                else if (args[i] == "--data")
                {
                    plus_data = true;
                }
                else if (args[i] == "--noplugin")
                {
                    noplugin = true;
                }
                else if (args[i] == "--install_ext")
                {
                    
                }
                else if (args[i] == "--install_COM")
                {

                }
            }

            if (plus_data)
                no_data = false;
            #endregion

            Localization.Language.Culture = new CultureInfo(ChameleonCoder.Properties.Settings.Default.Language);

            if (!noplugin)
                LoadPlugins();

            App.Current.Exit += ExitHandler;

            Gui = new MainWindow();

            if (!no_data)
                ParseDir(AppDir + "\\Data");

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
                foreach (XmlNode file_ref in (from XmlNode _ref in doc.DocumentElement.ChildNodes
                                              where _ref.Name == "file" && File.Exists(_ref.Value)
                                              select _ref))
                    ParseFile(file_ref.Value);

                foreach (XmlNode dir_ref in (from XmlNode _ref in doc.DocumentElement.ChildNodes
                                             where _ref.Name == "dir" && Directory.Exists(_ref.Value)
                                             select _ref))
                    ParseDir(dir_ref.Value);
            }
            else if (!error)
                AddResource(doc.DocumentElement, null);
        }

        private static void ParseDir(string dir)
        {
            var files = (Directory.GetFiles(dir, "*.ccp", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(dir, "*.ccm", SearchOption.AllDirectories));

            foreach (string file in files)
            {
                ParseFile(file);
            }
        }

        internal static bool AddResource(XmlNode node, IResource parent)
        {
            IResource resource;
            
            resource = ResourceTypeManager.CreateInstanceOf(node.Name, node, parent);
            if (resource == null)
                resource = ResourceTypeManager.CreateInstanceOf(node.Attributes["fallback"].Value, node);
            if (resource == null)
                return false;

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
            List<Type> components = new List<Type>(Assembly.GetEntryAssembly().GetTypes());
            foreach (string dll in Directory.GetFiles(AppDir + "\\Components", "*.dll"))
                components.AddRange(Assembly.LoadFrom(dll).GetTypes());

            foreach (Type component in components)
            {
                if (component.IsAbstract || component.IsInterface || component.IsNotPublic)
                    continue;

                if (component.GetInterface(typeof(IComponentProvider).FullName) != null)
                    (Activator.CreateInstance(component) as IComponentProvider).Init(ContentMemberManager.RegisterComponent, ResourceTypeManager.RegisterComponent);

                if (component.GetInterface(typeof(LanguageModules.ILanguageModule).FullName) != null)
                    LanguageModules.LanguageModuleHost.Add(component);

                if (component.GetInterface(typeof(Services.IService).FullName) != null)
                    Services.ServiceHost.Add(component);
            }
        }
    }
}
