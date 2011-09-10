namespace ChameleonCoder.Resources
{
    public class ResourceCollection : InstanceCollection<System.Guid, Interfaces.IResource>
    {
        public new void Add(Interfaces.IResource instance)
        {
            base.Add(instance.Identifier, instance);
        }
    }
}
