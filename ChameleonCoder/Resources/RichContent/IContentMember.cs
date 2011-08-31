
namespace ChameleonCoder.Resources.RichContent
{
    public interface IContentMember
    {
        /// <summary>
        /// the list of childMembers the content member has
        /// </summary>
        RichContentCollection childMembers { get; }

        /// <summary>
        /// gets the member's representation, e.g. in form of html, txt, ...
        /// </summary>
        /// <param name="param">optional information for the member</param>
        /// <returns>the member's representation</returns>
        object GetRepresentation(object param = null);

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
