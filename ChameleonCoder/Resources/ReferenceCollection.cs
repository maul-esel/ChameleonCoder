namespace ChameleonCoder.Resources
{
    public sealed class ReferenceCollection : InstanceCollection<System.Guid, ResourceReference>
    {
        public new void Add(ResourceReference instance)
        {
            base.Add(instance.Identifier, instance);
        }
    }
}
