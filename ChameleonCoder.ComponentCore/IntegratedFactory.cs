using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.ComponentCore.Resources;
using ChameleonCoder.Interaction;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;
using Res = ChameleonCoder.ComponentCore.Properties.Resources;

namespace ChameleonCoder.ComponentCore
{
    [CCPlugin]
    public sealed class IntegratedFactory : IResourceFactory
    {
        #region IPlugin

        public string About { get { return "© 2011 maul.esel - CC integrated resource types"; } }

        public string Author { get { return "maul.esel"; } }

        public string Description { get { return "provides the ChameleonCoder integrated resource types"; } }

        public ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/Images/logo.png")); } }

        public Guid Identifier { get { return new Guid("{e6662af6-d0fd-45bd-a2ab-8784eda3079d}"); } }

        public string Name { get { return "ChameleonCoder.ComponentCore resources"; } }

        public string Version { get { return "0.0.0.1"; } }

        public void Initialize()
        {
            Res.Culture = new CultureInfo(InformationProvider.Language);
            InformationProvider.LanguageChanged += (LCID) => Res.Culture = new CultureInfo((int)LCID);

            ResourceTypeManager.RegisterComponent(typeof(FileResource), FileResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(CodeResource), CodeResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(TaskResource), TaskResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(GroupResource), GroupResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(LibraryResource), LibraryResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(ProjectResource), ProjectResource.Alias, this);
        }

        public void Shutdown() { }

        #endregion

        #region IComponentFactory

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

        public ImageSource GetTypeIcon(Type type)
        {
            string name = null;
            if (type == typeof(FileResource))
                name = FileResource.Alias;
            else if (type == typeof(CodeResource))
                name = CodeResource.Alias;
            else if (type == typeof(LibraryResource))
                name = LibraryResource.Alias;
            else if (type == typeof(GroupResource))
                name = GroupResource.Alias;
            else if (type == typeof(ProjectResource))
                name = ProjectResource.Alias;
            else if (type == typeof(TaskResource))
                name = TaskResource.Alias;

            return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/" + name + ".png"));
        }

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

        public IDictionary<string, string> CreateResource(Type type, string name, IResource parent)
        {
            string parent_name = parent != null ? parent.Name : string.Empty;
            ResourceCreator creator = new ResourceCreator(type, parent_name, name);

            if (creator.ShowDialog() == true)
                return creator.GetXmlAttributes();
            return null;
        }

        public IEnumerable<Type> RegisteredTypes
        {
            get { return registeredTypesArray; }
        }

        Type[] registeredTypesArray = new Type[6] { typeof(FileResource), typeof(CodeResource), typeof(LibraryResource),
                                typeof(ProjectResource), typeof(TaskResource), typeof(GroupResource) };

        public IResource CreateInstance(Type resourceType, System.Xml.XmlElement data, IResource parent)
        {
            IResource resource = Activator.CreateInstance(resourceType) as IResource;

            resource.Initialize(data, parent);

            return resource;
        }

        #endregion
    }
}
