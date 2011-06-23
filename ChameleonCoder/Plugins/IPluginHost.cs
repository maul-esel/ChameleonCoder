using System;

namespace ChameleonCoder.Plugins
{
    public interface IPluginHost
    {
        void AddMetadata(Guid resource, string name, string value);

        void AddMetadata(Guid resource, string name, string value, bool isdefault, bool noconfig);

        void AddResource(Resources.Base.ResourceBase resource);

        void AddResource(Resources.Base.ResourceBase resource, Guid parent);

        Guid GetCurrentResource();

        int GetCurrentView();

        Resources.Base.ResourceBase GetResource(Guid id);

        System.Windows.Window GetWindow();
    }
}
