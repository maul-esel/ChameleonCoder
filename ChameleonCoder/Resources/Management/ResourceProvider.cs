using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources.Implementations;

namespace ChameleonCoder.Resources.Management
{
    public sealed class ResourceProvider : IComponentProvider
    {
        public void Init(Action<Type, string> registerContentMember, Action<Type, Base.ResourceTypeInfo> registerResourceType)
        {
            registerResourceType(typeof(LinkResource),
                new Base.ResourceTypeInfo("link", "link", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/link.png")), Brushes.LightGreen,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(FileResource),
                new Base.ResourceTypeInfo("file", "file", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/file.png")), Brushes.BurlyWood,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(CodeResource),
                new Base.ResourceTypeInfo("code", "source code", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/code.png")), Brushes.LightSalmon,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(LibraryResource),
                new Base.ResourceTypeInfo("library", "library", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/library.png")), Brushes.SandyBrown,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(ProjectResource),
                new Base.ResourceTypeInfo("project", "project", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/project.png")), Brushes.Khaki,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(TaskResource),
                new Base.ResourceTypeInfo("task", "task", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/task.png")), Brushes.LightCoral,
                    ResourceProvider.Create, ResourceProvider.Load));

            registerResourceType(typeof(GroupResource),
                new Base.ResourceTypeInfo("group", "group", new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")), Brushes.DodgerBlue,
                    ResourceProvider.Create, ResourceProvider.Load));
        }

        public static void Load(Interfaces.IResource resource)
        {

        }

        public static void Create(Type target, Action<Interfaces.IResource, Resources.Collections.ResourceCollection> register)
        {
            ResourceCreator creator = new ResourceCreator(target);
            creator.ShowDialog();
            if (creator.DialogResult != false)
            {
                Interfaces.IResource resource = Activator.CreateInstance(target, creator.GetXmlNode()) as Interfaces.IResource;
                register(resource, null);
            }
        }
    }
}
