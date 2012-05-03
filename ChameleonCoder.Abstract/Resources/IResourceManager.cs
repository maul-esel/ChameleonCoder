using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    [ComVisible(true), Guid("DE1624A2-2422-4624-85CA-464F3C087167"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IResourceManager : IAppComponent
    {
        void Add(IResource resource, IResource parent);
        void AddRange(IResource[] resources, IResource parent);
        void Remove(IResource resource);

        void RemoveAll();
        void Shutdown();

        IResource ActiveResource { get; }
        void Open(IResource resource);
        void Close();

        void Delete(IResource resource);
        void Move(IResource resource, IResource newParent);
        IResource Copy(IResource resource, IResource newParent);

        IResource[] ChildResources { get; }
        IResource[] Resources { get; }
        IResource GetResource(Guid id);

        IResource Create(Guid key, string name, IObservableStringDictionary attributes, IResource parent, Files.IDataFile file);

        Guid[] GetIdPath(IResource resource);
        IResource GetFromIdPath(Guid[] path);
        bool IsAncestorOf(IResource descendant, IResource ancestor);
        bool IsDescendantOf(IResource ancestor, IResource descendant);
        string GetDisplayPath(IResource resource, string separator);

        [Obsolete]
        IResource GetResourceFromDisplayPath(string path, string separator);

        #region events

        /// <summary>
        /// raised when a resource is going to be loaded
        /// </summary>
        event ResourceEventHandler ResourceLoad;

        /// <summary>
        /// raised when a resource was loaded
        /// </summary>
        event ResourceEventHandler ResourceLoaded;

        /// <summary>
        /// raised when a resource is going to be unloaded
        /// </summary>
        event ResourceEventHandler ResourceUnload;

        /// <summary>
        /// raised when a resource was unloaded
        /// </summary>
        event ResourceEventHandler ResourceUnloaded;

        #endregion
    }
}
