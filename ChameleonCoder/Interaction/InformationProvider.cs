using System;
using System.IO;
using System.Xml;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using Odyssey.Controls;

namespace ChameleonCoder.Interaction
{
    public delegate void ResourceEventHandler(IResource sender, EventArgs e);
    public delegate void LanguageModuleEventHandler(ILanguageModule sender, EventArgs e);
    public delegate void ServiceEventHandler(IService sender, EventArgs e);
    public delegate void SettingsEventHandler(object newValue);

    public static class InformationProvider
    {
        #region settings

        public static string ProgrammingDirectory { get { return Properties.Settings.Default.ProgrammingDir; } }
        public static int Language { get { return Properties.Settings.Default.Language; } }

        #endregion

        #region information

        public static string AppDir { get { return App.AppDir; } }
        public static string DataDir { get { return App.DataDir; } }

        #endregion

        #region tools

        public static void RegisterCodeGenerator(CodeGeneratorEventHandler clicked, ImageSource image, string text)
        {
            RibbonButton button = new RibbonButton() { Content = text, LargeImage = image, DataContext = clicked };
            button.Click += (s, e) =>
                {
                    CodeGeneratorEventArgs args = new CodeGeneratorEventArgs();
                    clicked(GetCurrentResource(), args);
                    if (!args.Handled)
                        InsertCode(args.Code);
                };
            App.Gui.CustomGroup1.Controls.Add(button);
        }

        public static void RegisterStubCreator()
        {
        } // IStubCreator

        #endregion

        #region templates

        public static void RegisterTemplate() { } // ITemplate

        public static void UnregisterTemplate(Guid key) { }

        public static bool IsTemplateRegistered(Guid key) { return false; }

        #endregion

        #region Editing

        public static void AppendCode(string code) { }

        public static void InsertCode(string code, int position) { }

        public static void InsertCode(string code) { } // use cursor position

        #endregion

        #region resource management

        public static IResource GetResourceInstance(Guid id)
        {
            return ResourceManager.GetList().GetInstance(id);
        }

        public static ResourceTypeInfo GetResourceTypeInfo(Type type)
        {
            return ResourceTypeManager.GetInfo(type);
        }

        public static IResource GetCurrentResource()
        {
            return ResourceManager.ActiveItem;
        }

        public static bool IsResourceTypeRegistered(string alias)
        {
            return ResourceTypeManager.IsRegistered(alias);
        }

        public static bool IsResourceTypeRegistered(Type type)
        {
            return ResourceTypeManager.IsRegistered(type);
        }

        public static void SetCurrentResource(IResource resource)
        {
            ResourceManager.Open(resource);
        }

        public static void AddResource(IResource resource, IResource parent)
        {
            ResourceManager.Add(resource, parent);
        }

        #endregion

        #region Metadata management

        public static void AddMetadata(this IResource resource, string name, string value)
        {
            resource.AddMetadata(name, value);
        }

        public static void DeleteMetadata(this IResource resource, string name)
        {
            resource.DeleteMetadata(name);
        }

        #endregion

        #region shared infrastructure
        public static string FindFreePath(string directory, string baseName, bool isFile)
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

        public static XmlElement CloneElement(XmlElement element, XmlDocument newOwner)
        {
            XmlElement newElement = newOwner.CreateElement(element.Name);
            foreach (XmlAttribute attr in element.Attributes)
            {
                XmlAttribute newAttr = newOwner.CreateAttribute(attr.Name);
                newAttr.Value = attr.Value;
                newElement.SetAttributeNode(newAttr);
            }
            foreach (XmlElement child in element.ChildNodes)
            {
                XmlElement newChild = CloneElement(child, newOwner);
                newElement.AppendChild(newChild);
            }
            return newElement;
        }
        #endregion

        #region events

        public static event ResourceEventHandler ResourceLoad;
        public static event ResourceEventHandler ResourceLoaded;

        public static event ResourceEventHandler ResourceUnload;
        public static event ResourceEventHandler ResourceUnloaded;

        public static event LanguageModuleEventHandler ModuleLoad;
        public static event LanguageModuleEventHandler ModuleLoaded;

        public static event LanguageModuleEventHandler ModuleUnload;
        public static event LanguageModuleEventHandler ModuleUnloaded;

        public static event ServiceEventHandler ServiceExecute;
        public static event ServiceEventHandler ServiceExecuted;

        public static event SettingsEventHandler LanguageChanged;
        public static event SettingsEventHandler ProgrammingDirChanged;

        #endregion

        #region event wrappers

        internal static void OnResourceLoad(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceLoad;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnResourceLoaded(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceLoaded;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnResourceUnload(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceUnload;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnResourceUnloaded(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceUnloaded;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnModuleLoad(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoad;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnModuleLoaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoaded;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnModuleUnload(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnload;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnModuleUnloaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnloaded;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnServiceExecute(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecute;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnServiceExecuted(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecuted;
            if (handler != null)
                handler(sender, e);
        }

        internal static void OnLanguageChanged()
        {
            SettingsEventHandler handler = LanguageChanged;
            if (handler != null)
                handler(Language);
        }

        internal static void OnProgrammingDirChanged()
        {
            SettingsEventHandler handler = ProgrammingDirChanged;
            if (handler != null)
                handler(Language);
        }

        #endregion
    }
}
