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
    public class CodeResource : FileResource, ICompilable, IEditable, IRichContentResource
    {
        /// <summary>
        /// creates a new instance of the CodeResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public CodeResource(XmlNode node, IResource parent)
            : base(node, parent)
        {
            this.compatibleLanguages = new List<Guid>();
            this.RichContent = new RichContentCollection();
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/code.png")); } }

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
            protected set
            {
                this.Xml.Attributes["compilation-path"].Value = value;
                this.OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        #region IEditable

        public string GetText()
        {
            try { return System.IO.File.ReadAllText(this.Path); }
            catch (System.IO.DirectoryNotFoundException) { }
            catch (System.IO.FileNotFoundException) { }
            return string.Empty;
        }

        public void SaveText(string text)
        {
            System.IO.File.WriteAllText(this.Path, text);
        }

        #endregion

        #region IEnumerable<T>

        public override IEnumerator<PropertyDescription> GetEnumerator()
        {
            IEnumerator<PropertyDescription> baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            string langName = null;
            try { langName = LanguageModuleHost.GetModule(this.language).LanguageName; }
            catch (NullReferenceException) { }

            yield return new PropertyDescription("language", langName, "source code") { IsReadOnly = true };

            string list = string.Empty;
            foreach (Guid lang in this.compatibleLanguages)
            {
                try { list += LanguageModuleHost.GetModule(lang).LanguageName + "; "; }
                catch (NullReferenceException) { }
            }

            yield return new PropertyDescription("compatible languages", list, "source code") { IsReadOnly = true };
            yield return new PropertyDescription("compilation path", this.compilationPath, "source code");
        }

        #endregion

        #region IRichContentResource

        public bool ValidateRichContent(IContentMember member)
        {
            return true;
        }

        public RichContentCollection RichContent { get; protected set; }

        #endregion
    }
}
