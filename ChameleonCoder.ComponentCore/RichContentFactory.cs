using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore
{
    [CCPlugin]
    public sealed class RichContentFactory : IRichContentFactory
    {
        #region IPlugin

        /// <summary>
        /// gets the 'about'-information for this plugin
        /// </summary>
        public string About { get { return "© 2011 maul.esel - CC integrated RichContent members"; } }

        /// <summary>
        /// gets the author(s) of this plugin
        /// </summary>
        public string Author { get { return "maul.esel"; } }

        /// <summary>
        /// gets a short description of this plugin
        /// </summary>
        public string Description { get { return "provides the ChameleonCoder integrated RichContent members"; } }

        /// <summary>
        /// gets an icon representing this plugin to the user
        /// </summary>
        public ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/Images/logo.png")); } }

        /// <summary>
        /// gets an globally unique identifier identifying the plugin
        /// </summary>
        public Guid Identifier { get { return new Guid("{3030d050-d7e5-4994-939f-39647a91ad65}"); } }

        /// <summary>
        /// gets the plugin's name
        /// </summary>
        public string Name { get { return "ChameleonCoder.ComponentCore RichContent members"; } }

        /// <summary>
        /// gets the plugin's current version
        /// </summary>
        public string Version { get { return "0.0.0.1"; } }

        /// <summary>
        /// initializes the plugin
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// prepares the plugin for closing the application
        /// </summary>
        public void Shutdown() { }

        #endregion

        public IContentMember CreateInstance(Type memberType, System.Xml.XmlElement data, IContentMember parent)
        {
            return null;
        }

        public IContentMember CreateMember(Type type, string name, IContentMember parent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> RegisteredTypes { get { return new Type[0] { }; } }
    }
}
