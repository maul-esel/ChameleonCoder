namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a collection class for <see cref="ChameleonCoder.Resources.ResourceReference"/> instances
    /// </summary>
    public sealed class ReferenceCollection : InstanceCollection<System.Guid, ResourceReference>
    {
        /// <summary>
        /// adds a new <see cref="ChameleonCoder.Resources.ResourceReference"/> instance to the collection
        /// </summary>
        /// <param name="instance">the instance to add</param>
        public new void Add(ResourceReference instance)
        {
            base.Add(instance.Identifier, instance);
        }
    }
}
