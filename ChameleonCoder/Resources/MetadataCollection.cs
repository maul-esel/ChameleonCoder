namespace ChameleonCoder.Resources
{
    public class MetadataCollection : InstanceCollection<string, Metadata>
    {
        public new void Add(Metadata data)
        {
            base.Add(data.Name, data);
        }
    }
}
