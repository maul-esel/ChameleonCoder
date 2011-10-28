using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a file containing source code
    /// </summary>
    public class CodeResource : FileResource, ICompilable, IRichContentResource
    {
        /// <summary>
        /// initializes the current instance with the given information
        /// </summary>
        /// <param name="data">the XmlElement containing the resource's definition</param>
        /// <param name="parent">the resource's parent resource,
        /// or null if the resource is a top-level resource.</param>
        public override void Update(XmlElement data, IResource parent)
        {
            base.Update(data, parent);
            // todo: parse compatible languages
        }

        #region IResource

        /// <summary>
        /// gets the icon that represents this instance to the user
        /// </summary>
        /// <value>This is always the same as the CodeResource's type icon.</value>
        public override ImageSource Icon
        {
            get
            {
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/code.png"))
                    .GetAsFrozen() as ImageSource;
            }
        }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// gets the identifier of the language module in whose coding language the source code is written.
        /// </summary>
        /// <value>The value is taken from the "language" attribute in the resource's XML.</value>
        public Guid Language
        {
            get
            {
                Guid lang;
                string guid = Xml.GetAttribute("language", DataFile.NamespaceUri);

                if (!Guid.TryParse(guid, out lang))
                    lang = Guid.Empty;

                return lang;
            }
            protected set
            {
                Xml.SetAttribute("language", DataFile.NamespaceUri, value.ToString("b"));
                OnPropertyChanged("Language");
            }
        }

        /// <summary>
        /// gets a list of Guid's identifying language modules
        /// to whose coding languages this resource is compatible
        /// </summary>
        /// <value>(not yet implemented)</value>
        public IEnumerable<Guid> CompatibleLanguages
        {
            get { return languages; }
        }

        private List<Guid> languages = new List<Guid>();

        #endregion

        #region ICompilable

        /// <summary>
        /// gets or sets the path to save the file if it is compiled.
        /// </summary>
        /// <value>The value is taken from the "compilation-path" attribute in the resource's XML.</value>
        [ResourceProperty(CommonResourceProperty.CompilationPath, ResourcePropertyGroup.ThisClass)]
        public string CompilationPath
        {
            get
            {
                string result = Xml.GetAttribute("compilation-path", DataFile.NamespaceUri);
                if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(Path))
                    result = Path + ".exe";
                return result;
            }
            set
            {
                Xml.SetAttribute("compilation-path", DataFile.NamespaceUri, value);
                OnPropertyChanged("CompilationPath");
            }
        }

        #endregion

        #region IRichContentResource

        /// <summary>
        /// gets the representation of the resource's RichContent
        /// </summary>
        /// <returns>the HTML as string</returns>
        public string GetHtml()
        {
            string markup = null;

            var list = System.Linq.Enumerable.OrderBy(RichContent, c => c.GetType().Name);
            foreach (var member in list)
            {
                markup += member.GetHtml(null);
            }

            return "<!DOCTYPE html>\n<html><head><style type='text/css'>"
                + StyleSheet
                + "</style></head><body style='margin: 2.5px; border: solid gray 2px; padding: 2.5px'>"
                + markup
                + "</body></html>";
        }

        /// <summary>
        /// registers a CSS class style
        /// </summary>
        /// <param name="classStyle">the style to register</param>
        public void RegisterClassStyle(ChameleonCoder.Resources.RichContent.Css.CssClassStyle classStyle)
        {
            StyleSheet += classStyle.ToString() + '\n';
        }

        private string StyleSheet = null;

        /// <summary>
        /// contains the resource's RichContent members
        /// </summary>
        public RichContentCollection RichContent
        {
            get { return collection; }
        }

        private RichContentCollection collection = new RichContentCollection();

        #endregion

        #region PropertyAliases

        /// <summary>
        /// gets the display name of this resource's language module
        /// </summary>
        [ResourceProperty(CommonResourceProperty.Language, ResourcePropertyGroup.ThisClass, IsReadOnly = true)]
        public string LanguageName
        {
            get
            {
                ILanguageModule module;
                if (PluginManager.TryGetModule(Language, out module))
                    return module.Name;
                return "error: module could not be found";
            }
        }

        /// <summary>
        /// gets a string contatining the dislay names of the language modules to which the source code is compatible.
        /// </summary>
        [ResourceProperty(CommonResourceProperty.CompatibleLanguages, ResourcePropertyGroup.ThisClass, IsReadOnly = true)]
        public string CompatibleLanguagesNames
        {
            get
            {
                string list = string.Empty;
                foreach (Guid lang in CompatibleLanguages)
                {
                    ILanguageModule module;
                    if (PluginManager.TryGetModule(lang, out module))
                        list += module.Name + "; ";
                }
                return list;
            }
        }

        #endregion

        internal new const string Key = "{478e8dd0-b37e-4616-8246-31984077bb64}";
    }
}
