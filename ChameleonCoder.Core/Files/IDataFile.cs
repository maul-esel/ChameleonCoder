using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Files
{
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("5DAC5D33-6E2F-4C05-B400-14340196B875")]
    public interface IDataFile
    {
        void Initialize(ChameleonCoderApp app);
        void Shutdown();

        void Open(string path);
        void Load();
        void Save();

        bool IsInitialized { get; }
        bool IsOpened { get; }
        bool IsLoaded { get; }

        void SetMetadata(string key, string value);
        string GetMetadata(string key);
        StringDictionary GetMetadata();
        void DeleteMetadata(string key);

        Guid AddFileReference(string path);
        Guid AddDirectoryReference(string path);
        string GetFileReference(Guid id);
        string GetDirectoryReference(Guid id);
        DataFileReference[] GetReferences();
        void DeleteReference(Guid id);

        void ResourceDelete(IResource resource);
        void ResourceInsert(IResource resource, IResource parent);

        void ResourceSetCreatedDate(IResource resource);
        void ResourceSetCreatedDate(IResource resource, DateTime time);
        DateTime ResourceGetCreatedDate(IResource resource);

        void ResourceUpdateLastModified(IResource resource);
        void ResourceUpdateLastModified(IResource resource, DateTime time);
        DateTime ResourceGetLastModified(IResource resource);

        void ResourceSetMetadata(IResource resource, string key, string value);
        string ResourceGetMetadata(IResource resource, string key);
        StringDictionary ResourceGetMetadata(IResource resource);
        void ResourceDeleteMetadata(IResource resource, string key);

        string FilePath { get; }
        string Name { get; }
        ChameleonCoderApp App { get; }
    }
}
