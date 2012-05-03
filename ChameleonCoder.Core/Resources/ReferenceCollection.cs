namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a collection class for <see cref="ChameleonCoder.Resources.ResourceReference"/> instances
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public sealed class ReferenceCollection : InstanceCollection<System.Guid, IResourceReference>
    {
        /// <summary>
        /// adds a new <see cref="ChameleonCoder.Resources.ResourceReference"/> instance to the collection
        /// </summary>
        /// <param name="instance">the instance to add</param>
        public new void Add(IResourceReference instance)
        {
            base.Add(instance.Identifier, instance);
        }
    }
}
