using System.Collections.Generic;

namespace ChameleonCoder.Resources
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

        public override string ToString()
        {
            string representation = ElementName + "." + ClassName;

            if (PseudoClass != null)
            {
                representation += ":" + PseudoClass.ToString();
            }
            foreach (var selector in NestedSelectors)
            {
                representation += " " + selector.ToString();
            }

            return representation;
        }

        #endregion
    }
}
