using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a delegate for resource events
    /// </summary>
    /// <param name="sender">the resource raising the event</param>
    /// <param name="e">additional data</param>
    public delegate void ResourceEventHandler(object sender, IResourceEventArgs e);

    [ComVisible(true), Guid("7361F774-ED10-452E-8C02-6C59B495C03E"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IResourceManagerEvents
    {
        void ResourceLoad(object sender, IResourceEventArgs e);
        void ResourceLoaded(object sender, IResourceEventArgs e);

        void ResourceUnload(object sender, IResourceEventArgs e);
        void ResourceUnloaded(object sender, IResourceEventArgs e);
    }
}
