using System;
using System.Windows.Media;

namespace ChameleonCoder.Resources.Base
{
    public sealed class ResourceTypeInfo
    {
        public ResourceTypeInfo(string Alias, string DisplayName, ImageSource TypeIcon, Brush Background,
            Func<Type, Interfaces.IResource> Creator, Action<Interfaces.IResource> Loader)
        {
            this.Alias = Alias;
            this.DisplayName = DisplayName;
            this.TypeIcon = TypeIcon;
            this.Background = Background;
            this.Creator = Creator;
            this.Loader = Loader;
        }

        public string Alias { get; private set; }

        public string DisplayName { get; private set; }

        public ImageSource TypeIcon { get; private set; }

        public Brush Background { get; private set; }

        public Func<Type, Interfaces.IResource> Creator { get; private set; }

        public Action<Interfaces.IResource> Loader { get; private set; }
    }
}
