using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// represents a function
    /// </summary>
    public class FunctionMember : MemberBase
    {
        /// <summary>
        /// creates a new instance of the FunctionMember class
        /// </summary>
        /// <param name="node">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        public FunctionMember(System.Xml.XmlElement node, IContentMember parent, IRichContentResource resource)
            : base(node, parent, resource)
        {
        }

        #region IContentMember

        /// <summary>
        /// gets the member's HTML representation
        /// </summary>
        /// <param name="param">not used.</param>
        /// <returns>the representation as HTML text</returns>
        public override string GetHtml(object param)
        {
            string Syntax = "<pre class='builtin-syntax'>" + Name + "({0})";

            string paramTable = null;
            string paramList = null;
            
            foreach (var child in Children)
            {
                var parameter = child as ParameterMember;
                if (parameter != null)
                {
                    paramTable += "<tr><td>" + parameter.Name + "</td><td><p>" + parameter.Summary + "</p><p>" + parameter.Description + "</p></td></tr>";
                    paramList += (string.IsNullOrWhiteSpace(parameter.Type) ? "" : parameter.Type + " ")
                        + parameter.Name
                        + (string.IsNullOrWhiteSpace(parameter.DefaultValue) ? "" : " = " + parameter.DefaultValue)
                        + ", ";
                }
            }
            paramList = paramList.TrimEnd(',', ' '); // remove last space + comma

            string representation = "<div class='builtin-container' id='" + Identifier.ToString("b") + "'>"
                + "<h3>" + ElementName + ": " + Name + "</h3>"
                + "<p>" + Summary + "</p><hr/>"
                + "<pre class='builtin-syntax'>" + HighlightCode(Name + "(" + paramList + ")") + "</pre>"
                + (string.IsNullOrWhiteSpace(paramTable) ? "" : "<table border='1' cellpadding='5px'><thead><tr><th>Name:</th><th>Description:</th></thead><tbody>" + paramTable + "</tbody></table><hr/>")
                + "<p>" + Description + "</p>"
                + (string.IsNullOrWhiteSpace(Example) ? null : "<pre class='builtin-example'>" + HighlightCode(Example) + "</pre>")
                + "</div>";
            return representation;
        }

        #endregion

        protected override string ElementName
        {
            get { return "Function"; }
        }

        internal const string Key = "{2fc4ddba-0af1-474b-8af7-3154103fa77e}";
    }
}
