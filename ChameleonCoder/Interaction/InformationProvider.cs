﻿using System;
using System.IO;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using Odyssey.Controls;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// a delegate for resource events
    /// </summary>
    /// <param name="sender">the resource raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void ResourceEventHandler(IResource sender, EventArgs args);

    /// <summary>
    /// a delegate for LanguageModule events
    /// </summary>
    /// <param name="sender">the LanguageModule raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void LanguageModuleEventHandler(ILanguageModule sender, EventArgs args);

    /// <summary>
    /// a delegate for Service Events
    /// </summary>
    /// <param name="sender">the service raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void ServiceEventHandler(IService sender, EventArgs args);

    /// <summary>
    /// a delegate for Settings events
    /// </summary>
    /// <param name="newValue">the setting's new value</param>
    public delegate void SettingsEventHandler(object newValue);

    /// <summary>
    /// a public class providing information and notification for plugins
    /// </summary>
    public static class InformationProvider
    {
        #region settings

        /// <summary>
        /// the user's programming directory
        /// </summary>
        public static string ProgrammingDirectory { get { return Properties.Settings.Default.ProgrammingDir; } }

        /// <summary>
        /// the user's language as LCID code
        /// </summary>
        public static int Language { get { return Properties.Settings.Default.Language; } }

        #endregion

        #region information

        /// <summary>
        /// the directory which contains the ChameleonCoder executable
        /// </summary>
        public static string AppDir { get { return App.AppDir; } }

        #endregion

        #region tools

        /// <summary>
        /// registers a new CodeGenerator
        /// </summary>
        /// <param name="clicked">a delegate to invoke when the generator is invoked</param>
        /// <param name="image">the image to display for the CodeGenerator</param>
        /// <param name="text">the name of the CodeGenerator</param>
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

        /// <summary>
        /// registers a new StubCreator
        /// </summary>
        public static void RegisterStubCreator()
        {
        } // IStubCreator

        #endregion

        #region Editing

        /// <summary>
        /// appends code to the currently edited resource
        /// </summary>
        /// <param name="code">the code to insert</param>
        public static void AppendCode(string code) { }

        /// <summary>
        /// inserts code in the currently edited resource
        /// </summary>
        /// <param name="code">the code to insert</param>
        /// <param name="position">the position to use</param>
        public static void InsertCode(string code, int position) { }

        /// <summary>
        /// inserts code in the currently edited resource at cursor position
        /// </summary>
        /// <param name="code">the code to insert</param>
        public static void InsertCode(string code) { } // use cursor position

        #endregion

        #region resource management

        /// <summary>
        /// gets the IResource instance, given the identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>the resource if it exists</returns>
        public static IResource GetResourceInstance(Guid id)
        {
            return ResourceManager.GetList().GetInstance(id);
        }

        /// <summary>
        /// gets the currently active resource
        /// </summary>
        /// <returns>the active IResource instance</returns>
        public static IResource GetCurrentResource()
        {
            return ResourceManager.ActiveItem;
        }

        /// <summary>
        /// tests if a resource type is registered, given its Xml-alias
        /// </summary>
        /// <param name="alias">the alias to test</param>
        /// <returns>true if a resource type is registered with this alias, false otherwise</returns>
        public static bool IsResourceTypeRegistered(string alias)
        {
            return ResourceTypeManager.IsRegistered(alias);
        }

        /// <summary>
        /// tests if a resource type is registered, given a Type instance
        /// </summary>
        /// <param name="type">the Type to test</param>
        /// <returns>true if the resource type is registered, false otherwise</returns>
        public static bool IsResourceTypeRegistered(Type type)
        {
            return ResourceTypeManager.IsRegistered(type);
        }

        /// <summary>
        /// opens a new resource
        /// </summary>
        /// <param name="resource">the resource to open</param>
        public static void SetCurrentResource(IResource resource)
        {
            ResourceManager.Open(resource);
        }

        /// <summary>
        /// adds a resource to the internal collection and to the parent collection.
        /// If it is a top-level resource, it is added to the collection of top-level resources.
        /// </summary>
        /// <param name="resource">the resource to add</param>
        /// <param name="parent">the parent resource or null if it is a top-level resource</param>
        public static void AddResource(IResource resource, IResource parent)
        {
            ResourceManager.Add(resource, parent);
        }

        #endregion

        #region Metadata management

        /// <summary>
        /// adds a metadata element to a resource
        /// </summary>
        /// <param name="resource">the resource to receive the metadata</param>
        /// <param name="name">the name of the metadata</param>
        /// <param name="value">the metadata's value</param>
        public static void AddMetadata(this IResource resource, string name, string value)
        {
            resource.AddMetadata(name, value);
        }

        /// <summary>
        /// deletes metadata from a resource
        /// </summary>
        /// <param name="resource">the resource to work on</param>
        /// <param name="name">the name of the metadata to delete</param>
        public static void DeleteMetadata(this IResource resource, string name)
        {
            resource.DeleteMetadata(name);
        }

        #endregion

        #region shared infrastructure

        /// <summary>
        /// finds a free path for a file or directory, given the directory and the base name.
        /// </summary>
        /// <param name="directory">the directory which should contain the file or directory</param>
        /// <param name="baseName">the base name for the new file or directory</param>
        /// <param name="isFile">true if the method should look for a free file path, false for a free directory path.</param>
        /// <returns></returns>
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

        #endregion

        #region events

        /// <summary>
        /// raised when a resource is going to be loaded
        /// </summary>
        public static event ResourceEventHandler ResourceLoad;

        /// <summary>
        /// raised when a resource was loaded
        /// </summary>
        public static event ResourceEventHandler ResourceLoaded;

        /// <summary>
        /// raised when a resource is going to be unloaded
        /// </summary>
        public static event ResourceEventHandler ResourceUnload;

        /// <summary>
        /// raised when a resource was unloaded
        /// </summary>
        public static event ResourceEventHandler ResourceUnloaded;

        /// <summary>
        /// raised when a language module is going to be loaded
        /// </summary>
        public static event LanguageModuleEventHandler ModuleLoad;

        /// <summary>
        /// raised when a language module was loaded
        /// </summary>
        public static event LanguageModuleEventHandler ModuleLoaded;

        /// <summary>
        /// raised when a language module is going to be unloaded
        /// </summary>
        public static event LanguageModuleEventHandler ModuleUnload;

        /// <summary>
        /// raised when a language module was unloaded
        /// </summary>
        public static event LanguageModuleEventHandler ModuleUnloaded;

        /// <summary>
        /// raised when a service is going to be executed
        /// </summary>
        public static event ServiceEventHandler ServiceExecute;

        /// <summary>
        /// raised when a service was executed
        /// </summary>
        public static event ServiceEventHandler ServiceExecuted;

        /// <summary>
        /// raised when the 'Language' setting changed
        /// </summary>
        public static event SettingsEventHandler LanguageChanged;

        /// <summary>
        /// raised when the 'ProgrammingDir' setting was changed
        /// </summary>
        public static event SettingsEventHandler ProgrammingDirChanged;

        #endregion

        #region event wrappers

        /// <summary>
        /// raises the ResourceLoad event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnResourceLoad(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceLoad;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ResourceLoaded event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnResourceLoaded(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceLoaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ResourceUnload event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnResourceUnload(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceUnload;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ResourceUnloaded event
        /// </summary>
        /// <param name="sender">the resource raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnResourceUnloaded(IResource sender, EventArgs e)
        {
            ResourceEventHandler handler = ResourceUnloaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleLoad event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnModuleLoad(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoad;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleLoaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnModuleLoaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleLoaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleUnload event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnModuleUnload(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnload;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ModuleUnloaded event
        /// </summary>
        /// <param name="sender">the module raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnModuleUnloaded(ILanguageModule sender, EventArgs e)
        {
            LanguageModuleEventHandler handler = ModuleUnloaded;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ServiceExecute event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnServiceExecute(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecute;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the ServiceExecuted event
        /// </summary>
        /// <param name="sender">the service raising the event</param>
        /// <param name="e">additional data</param>
        internal static void OnServiceExecuted(IService sender, EventArgs e)
        {
            ServiceEventHandler handler = ServiceExecuted;
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// raises the LanguageChanged event
        /// </summary>
        internal static void OnLanguageChanged()
        {
            SettingsEventHandler handler = LanguageChanged;
            if (handler != null)
                handler(Language);
        }

        /// <summary>
        /// raises the ProgrammingDirChanged event
        /// </summary>
        internal static void OnProgrammingDirChanged()
        {
            SettingsEventHandler handler = ProgrammingDirChanged;
            if (handler != null)
                handler(Language);
        }

        #endregion
    }
}
