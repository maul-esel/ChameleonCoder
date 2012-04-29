namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a collection class for <see cref="ChameleonCoder.Resources.Interfaces.IResource"/> instances
    /// This class is not exposed to COM, it's only for internal usage and usage by plugins.
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public sealed class ResourceCollection : InstanceCollection<System.Guid, IResource>
    {
        /// <summary>
        /// adds a new <see cref="ChameleonCoder.Resources.Interfaces.IResource"/> instance to the collection
        /// </summary>
        /// <param name="instance">the instance to add</param>
        public new void Add(IResource instance)
        {
            base.Add(instance.Identifier, instance);
        }

        public ResourceCollection()
            : base()
        {
        }

        public ResourceCollection(System.Collections.Generic.IEnumerable<IResource> resources)
            : this()
        {
            foreach (var resource in resources)
            {
                Add(resource);
            }
        }
    }
}
