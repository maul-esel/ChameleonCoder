using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    [ComVisible(true), Guid("962E4C9F-4B66-4BA6-A756-950E2C6A296E"), InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IResourceReference : IComponent
    {
        Files.IDataFile File { get; }

        System.Windows.Media.ImageSource SpecialVisualProperty { get; }

        System.Collections.Specialized.IObservableStringDictionary Attributes { get; }

        IResource Resolve();
    }
}
