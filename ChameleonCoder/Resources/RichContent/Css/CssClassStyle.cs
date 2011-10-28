using System.Collections.Generic;

namespace ChameleonCoder.Resources.RichContent.Css
{
    public struct CssClassStyle
    {
        #region properties

        private readonly IList<CssStyleSelector> selectors;

        private readonly IDictionary<string, string> rules;

        #endregion

        #region properties

        public IEnumerable<CssStyleSelector> Selectors { get { return selectors; } }

        public IDictionary<string, string> StyleRules { get { return rules; } }

        #endregion

        #region constructors

        public CssClassStyle(IEnumerable<CssStyleSelector> selectors, IDictionary<string, string> rules)
            : this()
        {
            this.selectors = new List<CssStyleSelector>();
            this.rules = new Dictionary<string, string>();

            if (selectors != null)
            {
                foreach (var selector in selectors)
                {
                    this.selectors.Add(selector);
                }
            }            
            if (rules != null)
            {
                foreach (var pair in rules)
                {
                    this.rules.Add(pair);
                }
            }
        }

        public CssClassStyle(CssStyleSelector selector, IDictionary<string, string> rules)
            : this()
        {
            this.selectors = new List<CssStyleSelector>();
            this.rules = new Dictionary<string, string>();

            this.selectors.Add(selector);

            if (rules != null)
            {
                foreach (var pair in rules)
                {
                    this.rules.Add(pair);
                }
            }
        }

        #endregion

        /// <summary>
        /// gets the CSS code for the instance
        /// </summary>
        /// <returns>the CSS representation</returns>
        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();

            foreach (var selector in Selectors)
            {
                builder.Append(selector.ToString());
                builder.Append(", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append(" {\n");

            foreach (var pair in rules)
            {
                builder.Append(pair.Key + ": " + pair.Value + ";\n");
            }
            builder.Append("}");

            return builder.ToString();
        }
    }
}
