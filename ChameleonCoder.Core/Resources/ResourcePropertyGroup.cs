namespace ChameleonCoder.Resources
{
    /// <summary>
    /// an enumeration defining in which property group to place a property
    /// with the <see cref="ChameleonCoder.Resources.ResourcePropertyAttribute"/> applied on it.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
    public enum ResourcePropertyGroup
    {
        /// <summary>
        /// place the property in the "General" group.
        /// </summary>
        /// <remarks>You should only use this for properties
        /// corresponding to <see cref="ChameleonCoder.Resources.Interfaces.IResource"/> properties.</remarks>
        General,

        /// <summary>
        /// the display name of the class declaring the property will be used
        /// </summary>
        /// <remarks>If the declaring type is not registered,
        /// the <see cref="CurrentClass"/> value will be used instead.</remarks>
        ThisClass,

        /// <summary>
        /// the display name of the class the resource is an instance of will be used.
        /// </summary>
        CurrentClass
    }
}
