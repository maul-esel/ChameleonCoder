namespace ChameleonCoder.Resources.RichContent
{
    public class RichContentCollection : InstanceCollection<string, IContentMember>
    {
        public new void Add(IContentMember member)
        {
            base.Add(member);
        }
    }
}
