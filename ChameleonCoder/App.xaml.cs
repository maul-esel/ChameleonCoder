using System;
using System.IO;
using System.Windows;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Base;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static MainWindow Gui;

        internal static Plugins.PluginHost Host;

        internal void Init(Object sender, StartupEventArgs e)
        {
            Gui = new MainWindow();

            App.Host = new Plugins.PluginHost();

            ListData();

            Gui.Show();
        }

        private static void ListData()
        {
            if (Directory.Exists(Environment.CurrentDirectory + "\\#Data"))
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\#Data", "*.xml");

                foreach (string file in files)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.Load(file);
                    AddResource(ref doc, file, "/resource", null);
                }
            }
        }

        private static void AddResource(ref XmlDocument xmlnav, string file, string xpath, ResourceCollection parent)
        {
            ResourceBase resource;
            int i;
            ResourceType type = (ResourceType)xmlnav.CreateNavigator().SelectSingleNode(xpath + "/@data-type").ValueAsInt;

            switch (type)
            {
                case ResourceType.resource:
                    throw new NotImplementedException("invalid element type in " + file + " at position " + xpath);

                case ResourceType.link:
                    resource = new LinkResource(ref xmlnav, xpath, file); break;

                case ResourceType.file:
                    resource = new FileResource(ref xmlnav, xpath, file); break;

                case ResourceType.code:
                    resource = new CodeResource(ref xmlnav, xpath, file); break;

                case ResourceType.library:
                    resource = new LibraryResource(ref xmlnav, xpath, file); break;

                case ResourceType.project:
                    resource = new ProjectResource(ref xmlnav, xpath, file); break;

                case ResourceType.task:
                    resource = new TaskResource(ref xmlnav, xpath, file); break;

                default:
                    throw new Exception("parsing error in file " + file + " at position " + xpath + ".\ncase:" + type);
            }

            ResourceManager.Add(resource, parent);

            i = 0;
            foreach (XmlNode xmlnav2 in xmlnav.SelectNodes(xpath + "/resource"))
            {
                i++;
                AddResource(ref xmlnav, file, xpath + "/resource[" + i + "]", resource.children);
            }
        }
    }
}
