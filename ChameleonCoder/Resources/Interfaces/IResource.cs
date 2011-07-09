using System;
using System.Windows.Media;
using ChameleonCoder.Resources.Collections;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource
    {
        ImageSource Icon { get; }

        Guid GUID { get; }

        void Save();

        IResource Create();

        bool AddRichContentMember(ChameleonCoder.RichContent.IContentMember member);

        void Package();

        string Name { get; }

        string Description { get; }

        ResourceCollection children { get; }

        ImageSource SpecialVisualProperty { get; }
    }
}
