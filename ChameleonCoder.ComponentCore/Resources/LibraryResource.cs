using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Resources;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a library resource,
    /// inherits from CodeResource
    /// </summary>
    public class LibraryResource : CodeResource
    {
        #region IResource

        /// <summary>
        /// gets the icon that represents this instance to the user
        /// </summary>
        /// <value>This is always the same as the LibraryResource's type icon.</value>
        public override ImageSource Icon
        {
            get
            {
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/library.png"))
                    .GetAsFrozen() as ImageSource;
            }
        }

        #endregion

        /// <summary>
        /// gets or sets the author of this library.
        /// </summary>
        /// <value>The value is taken from the "author" attribute in the resource's XML.</value>
        [ResourceProperty("NameOfAuthor", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Author
        {
            get
            {
                return Xml.GetAttribute("author", DataFile.NamespaceUri);
            }
            set
            {
                Xml.SetAttribute("author", DataFile.NamespaceUri, value);
                OnPropertyChanged("Author");
            }
        }

        /// <summary>
        /// gets or sets the license of this library.
        /// </summary>
        /// <value>The value is taken from the "license" attibute in the resource's XML.</value>
        [ResourceProperty("NameOfLicense", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string License
        {
            get
            {
                return Xml.GetAttribute("license", DataFile.NamespaceUri);
            }
            set
            {
                Xml.SetAttribute("license", DataFile.NamespaceUri, value);
                OnPropertyChanged("License");
            }
        }

        /// <summary>
        /// gets or sets the version of this library.
        /// </summary>
        /// <value>The value is taken from the "version" attribute in the resource's XML.</value>
        [ResourceProperty("NameOfVersion", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Version
        {
            get
            {
                return Xml.GetAttribute("version", DataFile.NamespaceUri);
            }
            set
            {
                Xml.SetAttribute("version", DataFile.NamespaceUri, value);
                OnPropertyChanged("Version");
            }
        }

        #region Alias

        /// <summary>
        /// gets the localized name of the <see cref="Author"/> property.
        /// </summary>
        /// <value>The value is taken from the localized resource file.</value>
        public static string NameOfAuthor
        {
            get { return Properties.Resources.Info_Author; }
        }

        /// <summary>
        /// gets the localized name of the <see cref="License"/> property.
        /// </summary>
        /// <value>The value is taken from the localized resource file.</value>
        public static string NameOfLicense
        {
            get { return Properties.Resources.Info_License; }
        }

        /// <summary>
        /// gets the localized name of the <see cref="Version"/> property.
        /// </summary>
        /// <value>The value is taken from the localized resource file.</value>
        public static string NameOfVersion
        {
            get { return Properties.Resources.Info_Version; }
        }

        #endregion

        internal new const string Key = "{4c6175ed-aed4-4b1d-b39d-f66f400f8d23}";
    }
}
