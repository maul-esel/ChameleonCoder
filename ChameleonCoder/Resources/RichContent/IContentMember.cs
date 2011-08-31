
namespace ChameleonCoder.Resources.RichContent
{
    public interface IContentMember
    {
        /// <summary>
        /// the list of childMembers the content member has
        /// </summary>
        RichContentCollection childMembers { get; }

        /// <summary>
        /// gets the member's html representation
        /// </summary>
        /// <param name="param">optional information for the member</param>
        /// <returns>the member's representation</returns>
        string GetHtml(object param = null);

        /// <summary>
        /// saves the instance
        /// </summary>
        void Save();

        /// <summary>
        /// initializes the instance
        /// </summary>
        /// <param name="node"></param>
        void Init(System.Xml.XmlNode node);
    }
}
