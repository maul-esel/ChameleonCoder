using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;
using ChameleonCoder.LanguageModules;

namespace ChameleonCoder.ResourceCore
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
        /// <param name="datafile">the file that contains the definition</param>
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
            this.compatibleLanguages = new List<Guid>();
            this.RichContent = new RichContentCollection();
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/code.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        public Guid language
        {
            get
            {
                try { return new Guid(this.Xml.Attributes["language"].Value); }
                catch (NullReferenceException) { return Guid.Empty; }
            }
            protected set
            {
                this.Xml.Attributes["language"].Value = value.ToString();
                this.OnPropertyChanged("language");
            }
        }

        public List<Guid> compatibleLanguages { get; set; }

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
                string result = null;

                try { result = this.Xml.Attributes["compilation-path"].Value; }
                catch (NullReferenceException) { }

                if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(this.Path))
                    result = this.Path + ".exe";
                return result;
            }
            set
            {
                this.Xml.Attributes["compilation-path"].Value = value;
                this.OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        #region IRichContentResource

        public bool ValidateRichContent(IContentMember member)
        {
            return true;
        }

        public RichContentCollection RichContent { get; protected set; }

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
    }
}
