using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a library resource,
    /// inherits from CodeResource
    /// </summary>
    public class LibraryResource : CodeResource
    {
        /// <summary>
        /// creates a new instance of the LibraryResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public LibraryResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/library.png")); } }

        #endregion

        #region IEnumerable

        public override IEnumerator<PropertyDescription> GetEnumerator()
        {
            IEnumerator<PropertyDescription>  baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            yield return new PropertyDescription("author", this.Author, "library");
            yield return new PropertyDescription("license", this.License, "library");
            yield return new PropertyDescription("version", this.Version, "library");
        }

        #endregion

        public string Author
        {
            get
            {
                try { return this.XMLNode.Attributes["author"].Value; }
                catch (NullReferenceException) { return null; }
            }
            protected set
            {
                this.XMLNode.Attributes["author"].Value = value;
                this.OnPropertyChanged("Author");
            }
        }

        public string License
        {
            get
            {
                try { return this.XMLNode.Attributes["license"].Value; }
                catch (NullReferenceException) { return null; }
            }
            protected set
            {
                this.XMLNode.Attributes["license"].Value = value;
                this.OnPropertyChanged("License");
            }
        }

        public string Version
        {
            get
            {
                try { return this.XMLNode.Attributes["version"].Value; }
                catch (NullReferenceException) { return null; }
            }
            protected set
            {
                this.XMLNode.Attributes["version"].Value = value;
                this.OnPropertyChanged("Version");
            }
        }
    }
}
