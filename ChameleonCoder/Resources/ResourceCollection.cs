namespace ChameleonCoder.Resources
{
    public class ResourceCollection : InstanceCollection<System.Guid, Interfaces.IResource>
    {
        public void Add(Interfaces.IResource instance)
        {
            base.Add(instance.GUID, instance);
        }
    }
}
