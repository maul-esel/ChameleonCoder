using System;
using System.Windows.Media;

namespace ChameleonCoder.Resources
{
    public sealed class ResourceTypeInfo
    {
        public ResourceTypeInfo(string Alias, string DisplayName, ImageSource TypeIcon, Brush Background,
            Func<Type, Interfaces.IResource, Interfaces.IResource> Create, Action<Interfaces.IResource> Loader, string author)
        {
            this.Alias = Alias;
            this.DisplayName = DisplayName;
            this.TypeIcon = TypeIcon;
            this.Background = Background;
            this.Create = Create;
            this.Loader = Loader;
            this.Author = author;
        }

        public string Alias { get; private set; }

        public string DisplayName { get; private set; }

        public ImageSource TypeIcon { get; private set; }

        public Brush Background { get; private set; }

        public Func<Type, Interfaces.IResource, Interfaces.IResource> Create { get; private set; }

        public Action<Interfaces.IResource> Loader { get; private set; }

        public string Author { get; private set; }
    }
}
