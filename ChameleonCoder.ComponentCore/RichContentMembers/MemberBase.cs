using System;
using System.Collections.Generic;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;
using ChameleonCoder.Resources.RichContent.Css;

namespace ChameleonCoder.ComponentCore.RichContentMembers
{
    /// <summary>
    /// serves as base class for RichContent types
    /// </summary>
    public abstract class MemberBase : IContentMember
    {
        /// <summary>
        /// a base constructor for inherited types
        /// </summary>
        /// <param name="data">the XmlElement representing the member</param>
        /// <param name="parent">the member's parent</param>
        /// <param name="resource">the resource the member belongs to</param>
        protected MemberBase(XmlElement data, IContentMember parent, IRichContentResource resource)
        {
            parentMember = parent;
            xmlData = data;

            Identifier = Guid.Parse(data.GetAttribute("id", DataFile.NamespaceUri));

            RegisterStyles(resource);
        }

        private readonly IContentMember parentMember;

        private readonly XmlElement xmlData;

        private readonly RichContentCollection childrenCollection = new RichContentCollection();


        protected XmlElement Xml { get { return xmlData; } }


        public IContentMember Parent { get { return parentMember; } }

        public RichContentCollection Children { get { return childrenCollection; } }

        public string Name
        {
            get
            {
                return xmlData.GetAttribute("name", DataFile.NamespaceUri);
            }
            set
            {
                xmlData.SetAttribute("name", DataFile.NamespaceUri, value);
            }
        }

        public Guid Identifier { get; private set; }

        public virtual System.Windows.Media.ImageSource Icon { get { return null; } }


        public abstract void Save();

        public abstract string GetHtml(object data);


        private void RegisterStyles(IRichContentResource resource)
        {
            #region pre.builtin-syntax
            var selector1 = new CssStyleSelector("builtin-syntax", "pre", null);

            var dict1 = new Dictionary<string, string>();
            dict1.Add("background-color", "#FFFFAA");
            dict1.Add("border", "solid #FFEE00 1px");
            dict1.Add("padding", "5px");

            resource.RegisterClassStyle(new CssClassStyle(selector1, dict1));
            #endregion

            #region pre.builtin-example
            var selector2 = new CssStyleSelector("builtin-example", "pre", null);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("background-color", "#DEDEDE");
            dict2.Add("border", "solid gray 1px");
            dict2.Add("padding", "5px");

            resource.RegisterClassStyle(new CssClassStyle(selector2, dict2));
            #endregion

            #region pre em.builtin-comment

            var commentSelectors = new CssStyleSelector[2]
            {
                new CssStyleSelector("builtin-syntax", "pre", null),
                new CssStyleSelector("builtin-example", "pre", null)
            };
            foreach (var selector in commentSelectors)
                selector.AddNestedSelector(new CssStyleSelector("builtin-comment", "em", null));
            
            var commentDict = new Dictionary<string, string>();
            commentDict.Add("color", "green");

            resource.RegisterClassStyle(new CssClassStyle(commentSelectors, commentDict));

            #endregion

            #region pre span.builtin-string

            var stringSelectors = new CssStyleSelector[2]
            {
                new CssStyleSelector("builtin-syntax", "pre", null),
                new CssStyleSelector("builtin-example", "pre", null)
            };
            foreach (var selector in stringSelectors)
                selector.AddNestedSelector(new CssStyleSelector("builtin-string", "span", null));

            var stringDict = new Dictionary<string, string>();
            stringDict.Add("color", "blue");
            stringDict.Add("font-style", "italic");

            resource.RegisterClassStyle(new CssClassStyle(stringSelectors, stringDict));

            #endregion

            #region pre span.builtin-string:before

            var beforeSelectors = new CssStyleSelector[2]
            {
                new CssStyleSelector("builtin-syntax", "pre", null),
                new CssStyleSelector("builtin-example", "pre", null)
            };
            foreach (var selector in beforeSelectors)
                selector.AddNestedSelector(new CssStyleSelector("builtin-string", "span", CssPseudoClass.Before));

            var beforeDict = new Dictionary<string, string>();
            beforeDict.Add("content", @"'\''");

            resource.RegisterClassStyle(new CssClassStyle(beforeSelectors, beforeDict));

            #endregion

            #region pre span.builtin-string:after

            var afterSelectors = new CssStyleSelector[2]
            {
                new CssStyleSelector("builtin-syntax", "pre", null),
                new CssStyleSelector("builtin-example", "pre", null)
            };
            foreach (var selector in afterSelectors)
                selector.AddNestedSelector(new CssStyleSelector("builtin-string", "span", CssPseudoClass.After));

            var afterDict = new Dictionary<string, string>();
            afterDict.Add("content", @"'\''");

            resource.RegisterClassStyle(new CssClassStyle(afterSelectors, afterDict));

            #endregion
        }
    }
}
