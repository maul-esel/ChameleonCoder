using System;

namespace ChameleonCoder.Plugins
{
    public interface ILanguageModule
    {
        void Initalize(IPluginHost Host);

        void Shutdown();

        void Compile(Guid resource);

        void Execute(Guid resource);

        void Load();

        void Unload();

        string NewFunction(Guid resource);

        string NewClass(Guid resource);

        string NewLabel(Guid resource);

        void CharTyped(Guid resource, char TypedChar);

        string Author { get; }
        string Version { get; }
        string About { get; }

        int APIVersion { get; }
        Guid Language { get; }
        string LanguageName { get; }

        bool SupportsFunctions { get; }
        bool SupportsClasses { get; }
        bool SupportsLabels { get; }
    }
}
