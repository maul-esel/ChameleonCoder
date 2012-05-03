using System.Runtime.InteropServices;

namespace ChameleonCoder
{
    [ComVisible(true), Guid("6ABC019C-F0CC-4201-A291-1ACB08BAD3A7"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IChameleonCoderApp
    {
        void Exit(int exitCode);

        Files.IFileManager FileMan { get; }
        Plugins.IPluginManager PluginMan { get; }
        Resources.IResourceManager ResourceMan { get; }
        //Resources.RichContent.IRichContentManager ContentMemberMan { get; }
        [System.Obsolete]
        Resources.IResourceTypeManager ResourceTypeMan { get; }

        void RegisterExtensions();
        void UnregisterExtensions();

        void InitWindow();
        void ShowWindow();
        void HideWindow();
    }
}
