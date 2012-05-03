using System.Runtime.InteropServices;

namespace ChameleonCoder.Files
{
    [ComVisible(true), Guid("5210A173-43CE-4A1D-B1FA-E7A42B242CB8"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IFileManager : IAppComponent
    {
        IDataFile OpenFile(string path);
        void OpenDirectory(string path);

        bool IsFileOpen(string path);
        bool IsDirectoryOpen(string path);

        void Remove(IDataFile file);
        void Shutdown();

        void SaveAll();
        void RemoveAll();

        IDataFile[] Files { get; }
        string[] FilePaths { get; }
        string[] Directories { get; }
    }
}
