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
        bool IsInitialized { get; }

        void Open(string path);
        bool IsOpened { get; }

        void Save();

        [Obsolete]
        bool IsLoaded { get; }
        [Obsolete]
        void Load();

        void SetMetadata(string key, string value);
        string GetMetadata(string key);
        StringDictionary GetMetadata();
        void DeleteMetadata(string key);

        // todo: remove reference identification by ID!
        [Obsolete]
        Guid AddFileReference(string path);
        [Obsolete]
        Guid AddDirectoryReference(string path);
        [Obsolete]
        string GetFileReference(Guid id);
        [Obsolete]
        string GetDirectoryReference(Guid id);
        [Obsolete]
        DataFileReference[] GetReferences();
        [Obsolete]
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
