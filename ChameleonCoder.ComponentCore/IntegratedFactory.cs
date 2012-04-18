using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.ComponentCore.Resources;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Shared;
using Res = ChameleonCoder.ComponentCore.Properties.Resources;

namespace ChameleonCoder.ComponentCore
{
    /// <summary>
    /// an IResourceFactory plugin providing and managing the standard resource types
    /// </summary>
    [CCPlugin]
    public sealed class IntegratedFactory : IResourceFactory
    {
        #region IPlugin

        /// <summary>
        /// gets the 'about'-information for this plugin
        /// </summary>
        public string About { get { return "© 2011 maul.esel - CC integrated resource types"; } }

        /// <summary>
        /// gets the author(s) of this plugin
        /// </summary>
        public string Author { get { return "maul.esel"; } }

        /// <summary>
        /// gets a short description of this plugin
        /// </summary>
        public string Description { get { return "provides the ChameleonCoder integrated resource types"; } }

        /// <summary>
        /// gets an icon representing this plugin to the user
        /// </summary>
        public ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/Images/logo.png")); } }

        /// <summary>
        /// gets an globally unique identifier identifying the plugin
        /// </summary>
        public Guid Identifier { get { return new Guid("{e6662af6-d0fd-45bd-a2ab-8784eda3079d}"); } }

        /// <summary>
        /// gets the plugin's name
        /// </summary>
        public string Name { get { return "ChameleonCoder.ComponentCore resources"; } }

        /// <summary>
        /// gets the plugin's current version
        /// </summary>
        public string Version { get { return "0.0.0.1"; } }

        /// <summary>
        /// initializes the plugin
        /// </summary>
        public void Initialize()
        {
            Res.Culture = new CultureInfo(InformationProvider.Language);
            InformationProvider.LanguageChanged += (LCID) => Res.Culture = new CultureInfo((int)LCID);

            ResourceTypeManager.RegisterComponent(typeof(FileResource), Guid.Parse(FileResource.Key), this);
            ResourceTypeManager.RegisterComponent(typeof(CodeResource), Guid.Parse(CodeResource.Key), this);
            ResourceTypeManager.RegisterComponent(typeof(TaskResource), Guid.Parse(TaskResource.Key), this);
            ResourceTypeManager.RegisterComponent(typeof(GroupResource), Guid.Parse(GroupResource.Key), this);
            ResourceTypeManager.RegisterComponent(typeof(LibraryResource), Guid.Parse(LibraryResource.Key), this);
            ResourceTypeManager.RegisterComponent(typeof(ProjectResource), Guid.Parse(ProjectResource.Key), this);
        }

        /// <summary>
        /// prepares the plugin for closing the application
        /// </summary>
        public void Shutdown() { }

        #endregion

        #region IResourceFactory

        /// <summary>
        /// gets the localized display name for the given type
        /// </summary>
        /// <param name="type">the resource type to get the name for</param>
        /// <returns>the localized name</returns>
        /// <remarks>For obtaining the current language, use <code>InformationProvider.Language</code></remarks>
        public string GetDisplayName(Type type)
        {
            string name = null;

            if (type == typeof(FileResource))
                name = Res.Display_File;
            else if (type == typeof(CodeResource))
                name = Res.Display_Code;
            else if (type == typeof(LibraryResource))
                name = Res.Display_Library;
            else if (type == typeof(GroupResource))
                name = Res.Display_Group;
            else if (type == typeof(ProjectResource))
                name = Res.Display_Project;
            else if (type == typeof(TaskResource))
                name = Res.Display_Task;

            return name;
        }

        /// <summary>
        /// gets the type icon for a resource type registered by this factory
        /// </summary>
        /// <param name="type">the resource type to get an icon for</param>
        /// <returns>the ImageSource instance representing the resource type</returns>
        public ImageSource GetTypeIcon(Type type)
        {
            string name = null;
            if (type == typeof(FileResource))
                name = "file";
            else if (type == typeof(CodeResource))
                name = "code";
            else if (type == typeof(LibraryResource))
                name = "library";
            else if (type == typeof(GroupResource))
                name = "group";
            else if (type == typeof(ProjectResource))
                name = "project";
            else if (type == typeof(TaskResource))
                name = "task";

            return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/" + name + ".png"));
        }

        /// <summary>
        /// gets the background brush for a resource type registered by this factory
        /// </summary>
        /// <param name="type">the resource type to get the brush for</param>
        /// <returns>the System.Windows.Media.Brush instance, which can be a SolidColorBrush, an ImageBrush,
        /// a GradientBrush, ...</returns>
        public Brush GetBackground(Type type)
        {
            Color top = Colors.White;
            Color bottom = Colors.White;

            if (type == typeof(FileResource))
                bottom = Colors.MediumBlue;
            else if (type == typeof(CodeResource))
                bottom = Colors.Orange;
            else if (type == typeof(LibraryResource))
                bottom = Colors.Sienna;
            else if (type == typeof(GroupResource))
                bottom = Colors.Maroon;
            else if (type == typeof(ProjectResource))
                bottom = Colors.Red;
            else if (type == typeof(TaskResource))
                bottom = Colors.Lime;

            var brush = new LinearGradientBrush(top, bottom,
                new System.Windows.Point(0.5, 0), new System.Windows.Point(0.5, 1)).GetAsFrozen() as Brush;

            return brush;
        }

        /// <summary>
        /// creates a blueprint for a new resource
        /// </summary>
        /// <param name="type">the resource type to create a 'blueprint' for</param>
        /// <param name="name">the new resource's name</param>
        /// <param name="parent">the new resource's parent resource</param>
        /// <returns>the 'blueprint' in form of a dictionary,
        /// containing the attributes the resource's XmlElement should have</returns>
        public IDictionary<string, string> CreateResource(Type type, string name, IResource parent)
        {
            string parent_name = parent != null ? parent.Name : string.Empty;
            ResourceCreator creator = new ResourceCreator(type, parent_name, name);

            if (creator.ShowDialog() == true)
                return creator.GetXmlAttributes();
            return null;
        }

        /// <summary>
        /// gets a list of all resource types registered by this factory
        /// </summary>
        public IEnumerable<Type> RegisteredTypes
        {
            get { return registeredTypesArray; }
        }

        private static Type[] registeredTypesArray = new Type[6] { typeof(FileResource), typeof(CodeResource),
                                                                   typeof(LibraryResource), typeof(ProjectResource),
                                                                   typeof(TaskResource), typeof(GroupResource) };

        /// <summary>
        /// creates a new instance of a resource type registered by this factory
        /// </summary>
        /// <param name="resourceType">the resource type to create an instance of</param>
        /// <param name="data">the XmlElement representing the resource</param>
        /// <param name="parent">the parent resource</param>
        /// <returns>the newly created instance</returns>
        public IResource CreateInstance(Type resourceType, System.Xml.XmlElement data, IResource parent, Files.DataFile file)
        {
            IResource resource = Activator.CreateInstance(resourceType) as IResource;

            resource.Update(data, parent, file);

            return resource;
        }

        #endregion
    }
}
