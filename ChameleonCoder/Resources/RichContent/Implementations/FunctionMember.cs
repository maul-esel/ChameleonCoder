namespace ChameleonCoder.Resources.RichContent.Implementations
{
    public class FunctionMember : IContentMember
    {
        #region IContentMember

        public string GetHtml(object param = null)
        {
            return string.Empty;
        }

        public RichContentCollection childMembers
        {
            get;
            private set;
        }

        public virtual void Save() { }

        public virtual void Init(System.Xml.XmlNode node) { }

        #endregion

        public FunctionMember()
        {
            this.childMembers = new RichContentCollection();
        }
    }
}
