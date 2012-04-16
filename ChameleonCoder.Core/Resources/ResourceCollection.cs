namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a collection class for <see cref="ChameleonCoder.Resources.Interfaces.IResource"/> instances
    /// </summary>
    public sealed class ResourceCollection : InstanceCollection<System.Guid, Interfaces.IResource>
    {
        /// <summary>
        /// adds a new <see cref="ChameleonCoder.Resources.Interfaces.IResource"/> instance to the collection
        /// </summary>
        /// <param name="instance">the instance to add</param>
        public new void Add(Interfaces.IResource instance)
        {
            base.Add(instance.Identifier, instance);
        }
    }
}
