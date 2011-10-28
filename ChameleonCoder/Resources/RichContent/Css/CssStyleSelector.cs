using System.Collections.Generic;

namespace ChameleonCoder.Resources.RichContent.Css
{
    public struct CssStyleSelector
    {
        #region fields

        private readonly string element;

        private readonly string _class;

        private readonly string pseudo; // use class with static properties instead

        private readonly IList<CssStyleSelector> nested;

        #endregion

        #region properties

        public string ElementName { get { return element; } }

        public string ClassName { get { return _class; } }

        public string PseudoClass { get { return pseudo; } }

        internal IEnumerable<CssStyleSelector> NestedSelectors { get { return nested; } }

        #endregion

        #region constructors

        /// <summary>
        /// creates a CssStyleSelector with the given class name
        /// </summary>
        /// <param name="classname">the name to use for the CSS class</param>
        public CssStyleSelector(string classname)
            : this(classname, null, null)
        {
        }

        /// <summary>
        /// creates a CssStyleSelector with the given information
        /// </summary>
        /// <param name="classname">the name to use for the CSS class</param>
        /// <param name="element">the element name (td, p, h1, h2, ...) for the style. Set to null to use none.</param>
        /// <param name="pseudo">the pseudo-class (link, visited, hover, ...)</param>
        public CssStyleSelector(string classname, string element, string pseudo)
            : this()
        {
            nested = new List<CssStyleSelector>();

            _class = classname;
            this.element = element;

            if (pseudo != null && CssPseudoClass.IsValid(pseudo))
                this.pseudo = pseudo;
        }

        #endregion

        #region methods

        public void AddNestedSelector(CssStyleSelector selector)
        {
            nested.Add(selector);
        }

        /// <summary>
        /// converts this instance into a string in CSS selector format
        /// </summary>
        /// <returns>the CSS selector string</returns>
        public override string ToString()
        {
            var builder = new System.Text.StringBuilder(ElementName + "." + ClassName);

            if (PseudoClass != null)
                builder.Append(":" + PseudoClass.ToString());

            int i = 0;
            foreach (var selector in NestedSelectors)
            {
                i++;
                builder.Append(" " + selector.ToString());

                if (i != nested.Count)
                    builder.Append(", ");
            }

            return builder.ToString();
        }

        #endregion
    }
}
