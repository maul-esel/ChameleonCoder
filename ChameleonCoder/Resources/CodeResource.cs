using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a file containing code,
    /// inherits from FileResource
    /// </summary>
    public class CodeResource : FileResource, ICompilable
    {
        /// <summary>
        /// creates a new instance of the CodeResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        internal CodeResource(ref XmlDocument xml, string xpath, string datafile)
            : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.code;
        }

        public CodeResource() { }

        #region IResource

        public override string Alias { get { return "code"; } }

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/code.png")); } }

        public override ImageSource TypeIcon { get { return this.Icon; } }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        public Guid language
        {
            get
            {
                try { return new Guid(this.XML.SelectSingleNode(this.XPath + "/@language").Value); }
                catch (NullReferenceException) { return Guid.Empty; }
            }
            protected internal set { this.XML.SelectSingleNode(this.XPath + "/@language").Value = value.ToString(); }
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
                result = this.XML.SelectSingleNode(this.XPath + "/@compilation-path").Value;
                if (string.IsNullOrWhiteSpace(result))
                    result = this.Path + ".exe";
                return result;
            }
            protected internal set { this.XML.SelectSingleNode(this.XPath + "/@compilation-path").Value = value; }
        }

        #endregion

        /// <summary>
        /// opens the resource, using the FileResource method + adding special information
        /// </summary>
        internal override void Open()
        {
            base.Open(); // as well as the base method, this currently doesn't work and should be done with data binding

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("CodeLanguage"), Plugins.PluginManager.GetLanguageName(this.Language) }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("CompilePath"), this.CompilationPath}));

            //App.Gui.listView2.Groups[1].Header = Localization.get_string("info_code");
        }

        /// <summary>
        /// creates a new CodeResource
        /// </summary>
        internal static new void Create()
        {

        }
    }
}
