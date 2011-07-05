using System;
using System.Windows.Media;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource
    {
        StaticInfo Info { get; }

        ImageSource Icon { get; }

        Guid GUID { get; }

        void Save();

        void Open();

        void Package();

        string Name { get; }

        string Description { get; }

        ResourceCollection children { get; }
    }
}
