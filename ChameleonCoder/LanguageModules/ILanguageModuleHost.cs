using System;
using System.Windows.Media;
using System.Windows;

namespace ChameleonCoder.LanguageModules
{
    public interface ILanguageModuleHost : IPluginHost
    {
        ICSharpCode.AvalonEdit.TextEditor GetEditControl();

        string GetCurrentEditText();

        void InsertCode(string code);

        void InsertCode(string code, int position);

        void RegisterCodeGenerator(Action<Guid, CodeGeneratorEventArgs> clicked, ImageSource image);

        void RegisterTool(); // not complete

        void RegisterStubCreator(int index, Action<Guid> clicked); // maybe an enum instead?

        void RegisterStubCreator(ImageSource pic, Action<Guid> clicked); // not complete

        void RegisterEditTool(); // not complete

        int MinSupportedAPIVersion { get; }
        int MaxSupportedAPIVersion { get; }
    }
}
