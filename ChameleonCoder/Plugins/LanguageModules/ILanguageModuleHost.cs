using System;
using System.Windows.Media;

namespace ChameleonCoder.Plugins.LanguageModules
{
    public interface ILanguageModuleHost : IPluginHost
    {
        void AddButton(string Text, ImageSource Image, System.Windows.RoutedEventHandler OnClick, int Panel);

        ICSharpCode.AvalonEdit.TextEditor GetEditControl();

        string GetCurrentEditText();

        void InsertCode(string code);

        void InsertCode(string code, int position);

        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
