using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Files
{
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("5DAC5D33-6E2F-4C05-B400-14340196B875")]
    public interface IDataFile : IAppComponent
    {
        void Initialize(IChameleonCoderApp app);
        void Shutdown();
        bool IsInitialized { get; }

        void Open(string path);
        bool IsOpened { get; }

        void Save();

        void SetMetadata(string key, string value);
        string GetMetadata(string key);
        IObservableStringDictionary GetMetadata();
        void DeleteMetadata(string key);

        void AddFileReference(string path);
        void AddDirectoryReference(string path);
        bool HasFileReference(string path);
        bool HasDirectoryReference(string path);
        string[] FileReferences { get; }
        string[] DirectoryReferences { get; }
        void DeleteFileReference(string path);
        void DeleteDirectoryReference(string path);

        void ResourceRemove(IResource resource);

        #region parsing

        IObservableStringDictionary[] ResourceParseChildren(IResource parent);
        IObservableStringDictionary[] ResourceParseReferences(IResource resource);
        IObservableStringDictionary[] ContentMemberParseChildren(IRichContentResource resource, Resources.RichContent.IContentMember member);

        #endregion

        void ResourceDelete(IResource resource);
        void ResourceInsert(IResource resource, IResource parent);
        void ResourceMove(IResource resource, IResource newParent);
        IResource ResourceCopy(IResource resource, IResource newParent);

        void ResourceSetCreatedDate(IResource resource);
        void ResourceSetCreatedDate(IResource resource, DateTime time);
        DateTime ResourceGetCreatedDate(IResource resource);

        void ResourceUpdateLastModified(IResource resource);
        void ResourceUpdateLastModified(IResource resource, DateTime time);
        DateTime ResourceGetLastModified(IResource resource);

        void ResourceSetMetadata(IResource resource, string key, string value);
        string ResourceGetMetadata(IResource resource, string key);
        IObservableStringDictionary ResourceGetMetadata(IResource resource);
        void ResourceDeleteMetadata(IResource resource, string key);

        string FilePath { get; }
        string Name { get; }
    }
}
