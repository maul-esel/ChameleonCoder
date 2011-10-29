using System;
using System.Windows.Media;

namespace ChameleonCoder
{
    /// <summary>
    /// a base interface for all external classes CC interacts with
    /// </summary>
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

        /// <summary>
        /// the component's description
        /// </summary>
        string Description { get; }
    }
}
