using System.Reflection;
using System.Text.RegularExpressions;

namespace ChameleonCoder.Resources.RichContent.Css
{
    public static class CssPseudoClass
    {
        #region fields

        [System.ContextStatic]
        public static readonly string Lang = @"lang\([a-zA-Z]{2}\)";

        [System.ContextStatic]
        public static readonly string NthChild = @"nth-child\((\dn+\d|odd|even|\d|n)\)";

        [System.ContextStatic]
        public static readonly string NthLastChild = @"nth-last-child\((\dn+\d|odd|even|\d|n)\)";

        [System.ContextStatic]
        public static readonly string NthOfType = @"nth-of-type\((\dn+\d|odd|even|\d|n)\)";

        [System.ContextStatic]
        public static readonly string NthLastOfType = @"nth-last-of-type\((\dn+\d|odd|even|\d|n)\)";

        [System.ContextStatic]
        public static readonly string Not = @"not\(.*\)";

        public static readonly string After = "after";
        public static readonly string Before = "before";

        public static readonly string Link = "link";
        public static readonly string Visited = "visited";

        public static readonly string Hover = "hover";
        public static readonly string Active = "active";
        public static readonly string Focus = "focus";

        public static readonly string Target = "target";        

        public static readonly string Enabled = "enabled";
        public static readonly string Disabled = "disabled";
        public static readonly string Checked = "checked";
        public static readonly string Indeterminate = "indeterminate";

        public static readonly string Root = "root";
        public static readonly string FirstChild = "first-child";
        public static readonly string LastChild = "last-child";
        public static readonly string OnlyChild = "only-child";

        public static readonly string FirstOfType = "first-of-type";
        public static readonly string LastOfType = "last-of-type";
        public static readonly string OnlyOfType = "only-of-type";

        public static readonly string Empty = "empty";

        #endregion

        /// <summary>
        /// tests a given string if it is a valid CSS pseudo class
        /// </summary>
        /// <param name="pseudo">the string to test</param>
        /// <returns>true if it is valid, false otherwise</returns>
        public static bool IsValid(string pseudo)
        {
            foreach (var field in typeof(CssPseudoClass).GetFields())
            {
                if (field.FieldType == typeof(string))
                {
                    string value = (string)field.GetValue(null);
                    if (field.IsDefined(typeof(System.ContextStaticAttribute), false))
                    {
                        return Regex.IsMatch(pseudo, value);
                    }
                    else
                    {
                        return value == pseudo;
                    }
                }
            }

            return false;
        }
    }
}
