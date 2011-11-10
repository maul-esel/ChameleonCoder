﻿using System;
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
            resourceAncestor = resource;
            xmlData = data;

            Identifier = Guid.Parse(data.GetAttribute("id", DataFile.NamespaceUri));

            RegisterStyles();

            var list = Xml.GetAttribute("related", DataFile.NamespaceUri);
            if (!string.IsNullOrWhiteSpace(list))
            {
                foreach (var entry in list.Split(new char[1] {' '}, StringSplitOptions.RemoveEmptyEntries))
                {
                    relatedIds.Add(Guid.Parse(entry));
                }
            }
        }

        private readonly IContentMember parentMember;

        private readonly IRichContentResource resourceAncestor;

        private readonly XmlElement xmlData;

        private readonly RichContentCollection childrenCollection = new RichContentCollection();

        private readonly List<Guid> relatedIds = new List<Guid>();

        /// <summary>
        /// the XmlElement representing the member
        /// </summary>
        protected XmlElement Xml { get { return xmlData; } }

        /// <summary>
        /// when overridden in a derived class, gets the member's type name
        /// </summary>
        protected abstract string ElementName { get; }

        /// <summary>
        /// when overridden in a derived class, gets the member's type
        /// </summary>
        protected abstract Plugins.Syntax.SyntaxElement Element { get; }

        /// <summary>
        /// gets the member's parent member
        /// </summary>
        public IContentMember Parent { get { return parentMember; } }

        /// <summary>
        /// gets the resource the member belongs to
        /// </summary>
        public IRichContentResource Resource { get { return resourceAncestor; } }

        /// <summary>
        /// gets the collection of child-members
        /// </summary>
        public RichContentCollection Children { get { return childrenCollection; } }

        /// <summary>
        /// gets a list of ids of related members
        /// </summary>
        public IEnumerable<Guid> Related { get { return relatedIds; } }

        /// <summary>
        /// gets a description for the member
        /// </summary>
        public string Description
        {
            get
            {
                return Xml.GetAttribute("description", DataFile.NamespaceUri);
            }
        }

        /// <summary>
        /// gets the member's name
        /// </summary>
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

        /// <summary>
        /// gets the user's summary for the member
        /// </summary>
        public string Summary
        {
            get { return Xml.GetAttribute("summary", DataFile.NamespaceUri); }
        }

        /// <summary>
        /// gets a code example if one exists
        /// </summary>
        public string Example
        {
            get { return Xml.GetAttribute("example", DataFile.NamespaceUri); }
        }

        /// <summary>
        /// uniquely identifies the member
        /// </summary>
        public Guid Identifier { get; private set; }

        public virtual System.Windows.Media.ImageSource Icon { get { return null; } }

        /// <summary>
        /// gets the member's representation
        /// </summary>
        /// <param name="data">an optional parameter</param>
        /// <returns>an HTML string representing the member</returns>
        public virtual string GetHtml(object data)
        {
            return "<div class='builtin-container' id='" + Identifier.ToString("b") + "'>"
                + "<h3>" + ElementName + ": " + Name + "</h3>"
                + "<p>" +  Summary + "</p><hr/>"
                + "<p>" + Description + "</p>"
                + (string.IsNullOrWhiteSpace(Example) ? null : "<hr/><pre class='builtin-example'>" + HighlightCode(Example) + "</pre>")
                + "</div>";
        }

        /// <summary>
        /// registers CSS style information to be used by the builtin members (or any others)
        /// </summary>
        private void RegisterStyles()
        {
            #region pre.builtin-syntax
            var selector1 = new CssStyleSelector("builtin-syntax", "pre", null);

            var dict1 = new Dictionary<string, string>();
            dict1.Add("background-color", "#FFFFAA");
            dict1.Add("border", "solid #FFEE00 1px");
            dict1.Add("padding", "5px");

            Resource.RegisterClassStyle(new CssClassStyle(selector1, dict1));
            #endregion

            #region pre.builtin-example
            var selector2 = new CssStyleSelector("builtin-example", "pre", null);

            var dict2 = new Dictionary<string, string>();
            dict2.Add("background-color", "#DEDEDE");
            dict2.Add("border", "solid gray 1px");
            dict2.Add("padding", "5px");

            Resource.RegisterClassStyle(new CssClassStyle(selector2, dict2));
            #endregion

            #region div.builtin-container
            var selector3 = new CssStyleSelector("builtin-container", "div", null);

            var dict3 = new Dictionary<string, string>();
            dict3.Add("border", "solid gray 1px");
            dict3.Add("padding", "5px");
            dict3.Add("margin", "2px");

            Resource.RegisterClassStyle(new CssClassStyle(selector3, dict3));
            #endregion
        }

        /// <summary>
        /// converts code into Html highlighted code, using the <see cref="ChameleonCoder.Shared.InformationProvider.HtmlColorizeCode"/> method.
        /// </summary>
        /// <param name="code">the code to highlight</param>
        /// <returns>the HTML representing the highlighted code if possible, the original code if highlighting fails</returns>
        protected string HighlightCode(string code)
        {
            var langResource = Resource as ILanguageResource;
            if (langResource != null)
            {
                if (Plugins.PluginManager.IsModuleRegistered(langResource.Language))
                {
                    var module = Plugins.PluginManager.GetModule(langResource.Language);
                    return Shared.InformationProvider.HtmlColorizeCode(code, module);
                }
            }
            return code;
        }

        /// <summary>
        /// querys the Resource for ILanguageResource, and if it is one, the module for IProvideSyntaxInfo
        /// </summary>
        /// <returns>the IrovideSyntaxInfo instance (or null on failure)</returns>
        protected Plugins.Syntax.IProvideSyntaxInfo QuerySyntaxModuleInterface()
        {
            var langResource = Resource as ILanguageResource;
            if (langResource != null)
            {
                if (Plugins.PluginManager.IsModuleRegistered(langResource.Language))
                {
                    return Plugins.PluginManager.GetModule(langResource.Language) as Plugins.Syntax.IProvideSyntaxInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// querys for the given SyntaxElement whether it is supported by the Resource's ILanguageModule
        /// </summary>
        /// <remarks>For this to work, the Resource must be an ILanguageResource and its module must be a IProvideSyntaxInfo</remarks>
        /// <param name="element">the element to test</param>
        /// <returns>true if supported, false if not supported, and null if querying fails</returns>
        protected bool? QuerySupported(Plugins.Syntax.SyntaxElement element)
        {
            var module = QuerySyntaxModuleInterface();
            if (module == null)
                return null;
            return module.IsSupported(element);
        }

        /// <summary>
        /// querys for the given SyntaxElement for the syntax used for it
        /// </summary>
        /// <remarks>For this to work, the Resource must be an ILanguageResource and its module must be a IProvideSyntaxInfo</remarks>
        /// <param name="element">the element to query for</param>
        /// <returns>a string array, or null if querying fails</returns>
        protected string[] QuerySyntax(Plugins.Syntax.SyntaxElement element)
        {
            var module = QuerySyntaxModuleInterface();
            if (module == null)
                return null;
            return module.GetSyntax(element);
        }
    }
}
