using System;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace ChameleonCoder.Resources
{
    [ComVisible(true), Guid("9C111DEC-64BE-465A-87A7-3A0533FDB9FD"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IResourceTypeManager : IAppComponent
    {
        Type GetResourceType(Guid key);
        Guid GetKey(Type type);
        Plugins.IResourceFactory GetFactory(Type component);
        string GetDisplayName(Type component);
        ImageSource GetTypeIcon(Type component);
        IResource CreateInstanceOf(Guid key, System.Collections.Specialized.IObservableStringDictionary data, IResource parent, Files.IDataFile file);
        void RegisterComponent(Type component, Guid key, Plugins.IResourceFactory factory);
        Brush GetBackground(Type component);
        bool IsRegistered(Type type);

        Type[] ResourceTypes { get; }
        Plugins.ITemplate GetDefaultTemplate(Type resourceType);
    }
}
