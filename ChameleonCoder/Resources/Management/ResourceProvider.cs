using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources.Implementations;

namespace ChameleonCoder.Resources.Management
{
    public sealed class ResourceProvider : IComponentProvider
    {
        public void Init(Action<Type, string> registerContentMember, Action<Type, StaticInfo> registerResourceType)
        {
            registerResourceType(typeof(LinkResource),
                new StaticInfo()
                {
                    Alias = "link",
                    Background = Brushes.CadetBlue,
                    DisplayName = "link",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/link.png")) });

            registerResourceType(typeof(FileResource),
                new StaticInfo() { Alias = "file",
                    Background = Brushes.Azure,
                    DisplayName = "file",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/file.png")) });

            registerResourceType(typeof(CodeResource),
                new StaticInfo() { Alias = "code",
                    Background = Brushes.AliceBlue,
                    DisplayName = "code",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/code.png")) });

            registerResourceType(typeof(LibraryResource),
                new StaticInfo() { Alias = "library",
                    Background = Brushes.Beige,
                    DisplayName = "library",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/library.png")) });

            registerResourceType(typeof(ProjectResource),
                new StaticInfo() { Alias = "project",
                    Background = Brushes.BurlyWood,
                    DisplayName = "project",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/project.png")) });

            registerResourceType(typeof(TaskResource),
                new StaticInfo() { Alias = "task",
                    Background = Brushes.Coral,
                    DisplayName = "task",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/task.png")) });
        }
    }
}
