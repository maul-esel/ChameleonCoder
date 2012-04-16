namespace ChameleonCoder.Resources
{
    /// <summary>
    /// defines common properties of resource types
    /// </summary>
    public enum CommonResourceProperty
    {
        /// <summary>
        /// the unique identifier of the resource
        /// </summary>
        Identifier,

        /// <summary>
        /// the name of the resource
        /// </summary>
        Name,

        /// <summary>
        /// the resource's description
        /// </summary>
        Description,

        /// <summary>
        /// the resource's parent
        /// </summary>
        Parent,

        /// <summary>
        /// the resource's file system path (for <see cref="ChameleonCoder.Resources.Interfaces.IFSComponent"/> resources)
        /// </summary>
        FSPath,

        /// <summary>
        /// the resource's coding language (for <see cref="ChameleonCoder.Resources.Interfaces.ILanguageResource"/> resources)
        /// </summary>
        Language,

        /// <summary>
        /// the resource's compatible languages (for <see cref="ChameleonCoder.Resources.Interfaces.ILanguageResource"/> resources)
        /// </summary>
        CompatibleLanguages
    }
}
