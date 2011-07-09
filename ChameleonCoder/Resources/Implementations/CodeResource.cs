using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a file containing code,
    /// inherits from FileResource
    /// </summary>
    public class CodeResource : FileResource, ICompilable, IEditable
    {
        /// <summary>
        /// creates a new instance of the CodeResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public CodeResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/code.png")); } }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        public Guid language
        {
            get
            {
                try { return new Guid(this.XMLNode.Attributes["language"].Value); }
                catch (NullReferenceException) { return Guid.Empty; }
            }
            protected internal set { this.XMLNode.Attributes["language"].Value = value.ToString(); }
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
                string result;
                result = this.XMLNode.Attributes["compilation-path"].Value;
                if (string.IsNullOrWhiteSpace(result))
                    result = this.Path + ".exe";
                return result;
            }
            protected internal set { this.XMLNode.Attributes["compilation-path"].Value = value; }
        }

        #endregion

        #region IEditable

        public string GetText()
        {
            return System.IO.File.ReadAllText(this.Path);
        }

        public void SaveText(string text)
        {
            System.IO.File.WriteAllText(this.Path, text);
        }

        #endregion
    }
}
