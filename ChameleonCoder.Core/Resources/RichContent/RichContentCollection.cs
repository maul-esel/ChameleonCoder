namespace ChameleonCoder.Resources.RichContent
{
    /// <summary>
    /// a collection class for <see cref="ChameleonCoder.Resources.RichContent.IContentMember"/> instances
    /// </summary>
    //[System.Runtime.InteropServices.ComVisible(false)]
    public class RichContentCollection : InstanceCollection<string, IContentMember>
    {
        /// <summary>
        /// adds a new instance to the collection
        /// </summary>
        /// <param name="member">the instance to add</param>
        public new void Add(IContentMember member)
        {
            base.Add(member);
        }
    }
}
