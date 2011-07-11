using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources.Implementations;

namespace ChameleonCoder.Resources.Management
{
    public sealed class ResourceProvider : IComponentProvider
    {
        public void Init(Action<Type, string> registerContentMember, Action<Type, Base.StaticInfo> registerResourceType)
        {
            registerResourceType(typeof(LinkResource),
                new Base.StaticInfo()
                {
                    Alias = "link",
                    Background = Brushes.LightGreen,
                    DisplayName = "link",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/link.png")) });

            registerResourceType(typeof(FileResource),
                new Base.StaticInfo()
                {
                    Alias = "file",
                    Background = Brushes.BurlyWood,
                    DisplayName = "file",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/file.png")) });

            registerResourceType(typeof(CodeResource),
                new Base.StaticInfo()
                {
                    Alias = "code",
                    Background = Brushes.LightSalmon,
                    DisplayName = "code",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/code.png")) });

            registerResourceType(typeof(LibraryResource),
                new Base.StaticInfo()
                {
                    Alias = "library",
                    Background = Brushes.SandyBrown,
                    DisplayName = "library",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/library.png")) });

            registerResourceType(typeof(ProjectResource),
                new Base.StaticInfo()
                {
                    Alias = "project",
                    Background = Brushes.Khaki,
                    DisplayName = "project",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/project.png")) });

            registerResourceType(typeof(TaskResource),
                new Base.StaticInfo()
                {
                    Alias = "task",
                    Background = Brushes.LightCoral,
                    DisplayName = "task",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/task.png")) });

            registerResourceType(typeof(GroupResource),
                new Base.StaticInfo()
                {
                    Alias = "group",
                    Background = Brushes.DodgerBlue,
                    DisplayName = "group",
                    TypeIcon = new BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/group.png")) });
        }
    }
}
