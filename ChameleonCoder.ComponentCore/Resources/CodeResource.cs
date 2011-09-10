using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a file containing code,
    /// inherits from FileResource
    /// </summary>
    public class CodeResource : FileResource, ICompilable, IRichContentResource
    {
        /// <summary>
        /// creates a new instance of the CodeResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
            compatibleLanguages = new List<Guid>();
            // todo: parse compatible languages
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/code.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        public Guid language
        {
            get
            {
                Guid lang;
                string guid = Xml.GetAttribute("language");

                if (!Guid.TryParse(guid, out lang))
                    lang = Guid.Empty;

                return lang;
            }
            protected set
            {
                Xml.SetAttribute("language", value.ToString());
                OnPropertyChanged("language");
            }
        }

        public List<Guid> compatibleLanguages { get; protected set; }

        #endregion

        #region ICompilable

        /// <summary>
        /// the path to save the file if it is compiled.
        /// </summary>
        [ResourceProperty(CommonResourceProperty.CompilationPath, ResourcePropertyGroup.ThisClass)]
        public string compilationPath
        {
            get
            {
                string result = Xml.GetAttribute("compilation-path");
                if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(Path))
                    result = Path + ".exe";
                return result;
            }
            set
            {
                Xml.SetAttribute("compilation-path", value);
                OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        #region IRichContentResource

        public string GetHtml()
        {
            return string.Empty;
        }

        public RichContentCollection RichContent
        {
            get { return collection; }
        }

        private RichContentCollection collection = new RichContentCollection();

        #endregion

        #region PropertyAliases

        [ResourceProperty(CommonResourceProperty.Language, ResourcePropertyGroup.ThisClass, IsReadOnly = true)]
        public string LanguageName
        {
            get
            {
                ILanguageModule module;
                if (PluginManager.TryGetModule(language, out module))
                    return module.Name;
                return "error: module could not be found";
            }
        }

        [ResourceProperty(CommonResourceProperty.CompatibleLanguages, ResourcePropertyGroup.ThisClass, IsReadOnly = true)]
        public string CompatibleLanguagesNames
        {
            get
            {
                string list = string.Empty;
                foreach (Guid lang in compatibleLanguages)
                {
                    ILanguageModule module;
                    if (PluginManager.TryGetModule(lang, out module))
                        list += module.Name + "; ";
                }
                return list;
            }
        }

        #endregion

        internal new const string Alias = "code";
    }
}
