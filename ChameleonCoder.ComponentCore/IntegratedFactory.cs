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
using ChameleonCoder.Resources.RichContent;
using Res = ChameleonCoder.ComponentCore.Properties.Resources;

namespace ChameleonCoder.ComponentCore
{
    [CCPlugin]
    public class IntegratedFactory : IComponentFactory
    {
        #region IPlugin

        public string About { get { return "© 2011 maul.esel - CC integrated resource types"; } }

        public string Author { get { return "maul.esel"; } }

        public string Description { get { return "provides the ChameleonCoder integrated resource and RichContent types"; } }

        public ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/Images/logo.png")); } }

        public Guid Identifier { get { return new Guid("{e6662af6-d0fd-45bd-a2ab-8784eda3079d}"); } }

        public string Name { get { return "ChameleonCoder.ResourceCore"; } }

        public string Version { get { return "0.0.0.1"; } }

        public void Initialize()
        {
            Res.Culture = new CultureInfo(InformationProvider.Language);
            InformationProvider.LanguageChanged += (LCID) => Res.Culture = new CultureInfo((int)LCID);

            ResourceTypeManager.RegisterComponent(typeof(FileResource), FileResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(CodeResource), CodeResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(TaskResource), TaskResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(LinkResource), LinkResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(GroupResource), GroupResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(LibraryResource), LibraryResource.Alias, this);
            ResourceTypeManager.RegisterComponent(typeof(ProjectResource), ProjectResource.Alias, this);
        }

        public void Shutdown() { }

        #endregion

        #region IComponentFactory

        public string GetDisplayName(Type component)
        {
            string name = null;

            if (component == typeof(FileResource))
                name = Res.Display_File;
            else if (component == typeof(CodeResource))
                name = Res.Display_Code;
            else if (component == typeof(LibraryResource))
                name = Res.Display_Library;
            else if (component == typeof(GroupResource))
                name = Res.Display_Group;
            else if (component == typeof(LinkResource))
                name = Res.Display_Link;
            else if (component == typeof(ProjectResource))
                name = Res.Display_Project;
            else if (component == typeof(TaskResource))
                name = Res.Display_Task;

            return name;
        }

        public ImageSource GetTypeIcon(Type component)
        {
            string name = null;
            if (component == typeof(FileResource))
                name = FileResource.Alias;
            else if (component == typeof(CodeResource))
                name = CodeResource.Alias;
            else if (component == typeof(LibraryResource))
                name = LibraryResource.Alias;
            else if (component == typeof(GroupResource))
                name = GroupResource.Alias;
            else if (component == typeof(LinkResource))
                name = LinkResource.Alias;
            else if (component == typeof(ProjectResource))
                name = ProjectResource.Alias;
            else if (component == typeof(TaskResource))
                name = TaskResource.Alias;

            BitmapImage image = null;
            try
            {
                image = new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/" + name + ".png"));
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
            }
            return image;
        }

        public Brush GetBackground(Type component)
        {
            Color top = Colors.White;
            Color bottom = Colors.White;

            if (component == typeof(FileResource))
                bottom = Colors.MediumBlue;
            else if (component == typeof(CodeResource))
                bottom = Colors.Orange;
            else if (component == typeof(LibraryResource))
                bottom = Colors.Sienna;
            else if (component == typeof(GroupResource))
                bottom = Colors.Maroon;
            else if (component == typeof(LinkResource))
                bottom = Colors.Aqua;
            else if (component == typeof(ProjectResource))
                bottom = Colors.Red;
            else if (component == typeof(TaskResource))
                bottom = Colors.Lime;

            var brush = new LinearGradientBrush(top, bottom,
                new System.Windows.Point(0.5, 0), new System.Windows.Point(0.5, 1)).GetAsFrozen() as Brush;

            return brush;
        }

        public IEnumerable<KeyValuePair<string, string>> CreateResource(Type target, string name, IResource parent)
        {
            string parent_name = parent != null ? parent.Name : string.Empty;
            ResourceCreator creator = new ResourceCreator(target, parent_name, name);

            if (creator.ShowDialog() == true)
                return creator.GetXmlAttributes();
            return null;
        }

        public IContentMember CreateMember(Type type, string name, IContentMember parent)
        {
            return null;
        }

        public Type[] GetRegisteredTypes()
        {
            return new Type[7] { typeof(FileResource), typeof(CodeResource), typeof(LibraryResource),
                                typeof(LinkResource), typeof(ProjectResource), typeof(TaskResource),
                                typeof(GroupResource) };
        }

        #endregion
    }
}
