using System;
using System.Windows.Media;
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

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/library.png")).GetAsFrozen() as ImageSource; } }

        #endregion

        [ResourceProperty("NameOfAuthor", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Author
        {
            get
            {
                return Xml.GetAttribute("author");
            }
            set
            {
                Xml.SetAttribute("author", value);
                OnPropertyChanged("Author");
            }
        }

        [ResourceProperty("NameOfLicense", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string License
        {
            get
            {
                return Xml.GetAttribute("license");
            }
            set
            {
                Xml.SetAttribute("license", value);
                OnPropertyChanged("License");
            }
        }

        [ResourceProperty("NameOfVersion", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string Version
        {
            get
            {
                return Xml.GetAttribute("version");
            }
            set
            {
                Xml.SetAttribute("version", value);
                OnPropertyChanged("Version");
            }
        }

        #region Alias

        public static string NameOfAuthor
        {
            get { return Properties.Resources.Info_Author; }
        }

        public static string NameOfLicense
        {
            get { return Properties.Resources.Info_License; }
        }

        public static string NameOfVersion
        {
            get { return Properties.Resources.Info_Version; }
        }

        #endregion

        internal new const string Alias = "library";
    }
}
