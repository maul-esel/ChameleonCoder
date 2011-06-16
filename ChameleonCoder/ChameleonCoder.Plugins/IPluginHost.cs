using System;
using System.Windows.Media;

namespace ChameleonCoder.Plugins
{
    public interface IPluginHost
    {
        void InsertCode(string code);

        void InsertCode(string code, int position);

        void AddMetadata(Guid resource, string name, string value);

        void AddMetadata(Guid resource, string name, string value, bool isdefault, bool noconfig);

        void AddButton(string Text, ImageSource Image, Action<object, EventArgs> OnClick, int Panel);

        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
