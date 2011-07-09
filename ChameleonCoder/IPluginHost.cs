using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder
{
    public interface IPluginHost
    {
        void AddMetadata(Guid resource, string name, string value);

        void AddMetadata(Guid resource, string name, string value, bool isdefault, bool noconfig);

        void AddResource(IResource resource);

        void AddResource(IResource resource, Guid parent);

        Guid GetCurrentResource();

        int GetCurrentView();

        IResource GetResource(Guid id);
    }
}
