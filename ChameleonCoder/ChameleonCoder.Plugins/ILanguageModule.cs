using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.Plugins
{
    public interface ILanguageModule
    {
        void Initalize();

        void Shutdown();

        void Compile(Guid resource);

        void Execute(Guid resource);

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
