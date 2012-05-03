using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Files;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a file containing source code
    /// </summary>
    public class CodeResource : FileResource, IRichContentResource
    {
        /// <summary>
        /// initializes the current instance with the given information
        /// </summary>
        /// <param name="data">the dictionary containing the resource's attributes</param>
        /// <param name="parent">the resource's parent resource,
        /// or null if the resource is a top-level resource.</param>
        public override void Update(System.Collections.Specialized.IObservableStringDictionary data, IResource parent, IDataFile file)
        {
            base.Update(data, parent, file);
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
                string guid = Attributes["language"];

                if (!Guid.TryParse(guid, out lang))
                    lang = Guid.Empty;

                return lang;
            }
            protected set
            {
                Attributes["language"] = value.ToString("b"); // Xml.SetAttribute("language", XmlDataFile.NamespaceUri, value.ToString("b"));
                OnPropertyChanged("Language");
            }
        }

        /// <summary>
        /// gets a list of Guid's identifying language modules
        /// to whose coding languages this resource is compatible
        /// </summary>
        /// <value>(not yet implemented)</value>
        public Guid[] CompatibleLanguages
        {
            get { return languages.ToArray(); }
        }

        private List<Guid> languages = new List<Guid>();

        #endregion

        #region IRichContentResource

        /// <summary>
        /// gets the representation of the resource's RichContent
        /// </summary>
        /// <returns>the HTML as string</returns>
        public string GetHtml()
        {
            return string.Empty;
        }

        /// <summary>
        /// contains the resource's RichContent members
        /// </summary>
        public IContentMember[] RichContent
        {
            get { return collection.Values; }
        }

        public void AddContentMember(IContentMember member)
        {
            collection.Add(member);
        }

        public void RemoveContentMember(IContentMember member)
        {
            collection.Remove(member);
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
                if (File.App.PluginMan.IsModuleRegistered(Language))
                {
                    ILanguageModule module = File.App.PluginMan.GetModule(Language);
                    if (module != null)
                        return module.Name;
                }
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
                    if (File.App.PluginMan.IsModuleRegistered(lang))
                    {
                        ILanguageModule module = File.App.PluginMan.GetModule(lang);
                        if (module != null)
                            list += module.Name + "; ";
                    }
                }
                return list;
            }
        }

        #endregion

        internal new const string Key = "{478e8dd0-b37e-4616-8246-31984077bb64}";
    }
}
