using System;
using System.Windows;
using System.Windows.Media;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an interface the PluginManager implements. This can be used by plugins to call its functions
    /// </summary>
    public interface IManager
    {
        System.Globalization.CultureInfo GetUILanguage();

        void InsertCode(string code);
        void AddButton(string Text, ImageSource image, RoutedEventHandler OnClick, int UIContext);
        void AddMetadata(Guid resource, string name, string value);

        // todo: include all functions PluginManager offers to plugins
    }
}
