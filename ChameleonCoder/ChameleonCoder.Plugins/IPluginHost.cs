using System;
using System.Windows.Media;

namespace ChameleonCoder.Plugins
{
    public interface IPluginHost
    {
        Guid GetCurrentResource();

        int GetCurrentView();

        Resources.Base.ResourceBase GetResource(Guid id);

        void InsertCode(string code);

        void InsertCode(string code, int position);

        void AddButton(string Text, ImageSource Image, Action<object, EventArgs> OnClick, int Panel);

        void AddMetadata(Guid resource, string name, string value);

        void AddMetadata(Guid resource, string name, string value, bool isdefault, bool noconfig);

        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
