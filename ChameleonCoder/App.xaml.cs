using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using ChameleonCoder.Resources.Collections;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static MainWindow Gui;

        internal void Init(Object sender, StartupEventArgs e)
        {
            Localization.Language.Culture = new CultureInfo(ChameleonCoder.Properties.Settings.Default.Language);

            LoadPlugins();

            App.Current.Exit += ExitHandler;

            Gui = new MainWindow();

            Gui.Resources.Add("ResourceTypes", ResourceTypeManager.GetResourceTypes());

            ListData();

            Gui.Show();
        }

        internal static void ExitHandler(object sender, EventArgs e)
        {
            LanguageModules.LanguageModuleHost.Shutdown();
            Services.ServiceHost.Shutdown();
        }

        private static void ListData()
        {
            if (Directory.Exists(Environment.CurrentDirectory + "\\Data"))
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Data", "*.xml");

                foreach (string file in files)
                {
                    XmlDocument doc = new XmlDocument();

                    try { doc.Load(file); }
                    catch (XmlException e) { MessageBox.Show(file + "\n\n" + e.Message); continue; }

                    AddResource(doc.DocumentElement, null);
                }
            }
        }

        internal static bool AddResource(XmlNode node, ResourceCollection parent)
        {
            IResource resource;
            
            resource = ResourceTypeManager.CreateInstanceOf(node.Name, node);
            if (resource == null)
                resource = ResourceTypeManager.CreateInstanceOf(node.Attributes["fallback"].Value, node);
            if (resource == null)
                return false;

            IRichContentResource richResource = resource as IRichContentResource;
            IAllowChildren parentResource = resource as IAllowChildren;

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
                        {
                            if (richResource.ValidateRichContent(richContent))
                                resource.ToString(); // add to RichContentCollection
                        }
                    }
                else if (parentResource != null)
                    AddResource(child, parentResource.children);
            }

            return true;
        }

        private static void LoadPlugins()
        {
            List<Type> components = new List<Type>(Assembly.GetEntryAssembly().GetTypes());
            foreach (string dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Components", "*.dll"))
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
