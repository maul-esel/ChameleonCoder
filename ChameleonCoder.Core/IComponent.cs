using System;
using System.Windows.Media;
using System.Runtime.InteropServices;

namespace ChameleonCoder
{
    /// <summary>
    /// a base interface for all external classes CC interacts with
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("dc59c137-2edf-4772-86c8-1fc3028e90df")]
    public interface IComponent
    {
        /// <summary>
        /// an icon displayed along with the component's name
        /// </summary>
        ImageSource Icon { get; }

        /// <summary>
        /// a Guid uniquely identifying the component
        /// </summary>
        Guid Identifier { get; }

        /// <summary>
        /// the component's display name
        /// </summary>
        string Name { get; }
    }
}
