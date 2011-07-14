﻿using System;
using System.Windows.Media;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder.Resources.Base
{
    public sealed class ResourceTypeInfo
    {
        public ResourceTypeInfo(string Alias, string DisplayName, ImageSource TypeIcon, Brush Background,
            Action<Type, Action<Interfaces.IResource, ResourceCollection>> Creator, Action<Interfaces.IResource> Loader)
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

        public Action<Type, Action<Interfaces.IResource, ResourceCollection>> Creator { get; private set; }

        public Action<Interfaces.IResource> Loader { get; private set; }
    }
}