using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Text;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource
    {
        string Alias { get; }

        string DisplayName { get; }

        Guid GUID { get; }

        ImageSource TypeIcon { get; }

        ImageSource Icon { get; }

        void Save();

        void Open();

        void Package();

        string Name { get; }

        string Description { get; }
    }
}
