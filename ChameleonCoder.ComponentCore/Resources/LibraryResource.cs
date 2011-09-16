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

        [ResourceProperty("nameof_Author", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
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

        [ResourceProperty("nameof_License", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
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

        [ResourceProperty("nameof_Version", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
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

        public static string nameof_Author
        {
            get { return Properties.Resources.Info_Author; }
        }

        public static string nameof_License
        {
            get { return Properties.Resources.Info_License; }
        }

        public static string nameof_Version
        {
            get { return Properties.Resources.Info_Version; }
        }

        #endregion

        internal new const string Alias = "library";
    }
}
