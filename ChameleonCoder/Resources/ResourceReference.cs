using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a reference to another resource
    /// </summary>
    public sealed class ResourceReference : IComponent
    {
        /// <summary>
        /// creates a new instance of the ResourceReference class
        /// </summary>
        /// <param name="data">the XmlElement representing the reference</param>
        internal ResourceReference(System.Xml.XmlElement data)
        {
            Xml = data;

            Identifier = Guid.Parse(Xml.GetAttribute("id"));
            Target = Guid.Parse(Xml.GetAttribute("target"));
        }

        /// <summary>
        /// implements IComponent.Icon
        /// </summary>
        public ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder;component/Images/reference.png")).GetAsFrozen() as ImageSource; }
        }

        /// <summary>
        /// implements IComponent.Identifier
        /// </summary>
        public Guid Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// implements IComponent.Name
        /// </summary>
        public string Name
        {
            get
            {
                return Xml.GetAttribute("name");
            }
        }

        /// <summary>
        /// contains the Identifier of the target resource
        /// </summary>
        private Guid Target
        {
            get;
            set;
        }

        /// <summary>
        /// contains the XmlElement representing the reference
        /// </summary>
        public System.Xml.XmlElement Xml
        {
            get;
            private set;
        }

        /// <summary>
        /// gets the resource referenced by this instance
        /// </summary>
        /// <returns>the IResource instance for the resource</returns>
        public Interfaces.IResource Resolve()
        {
            return Management.ResourceManager.GetResource(Target);
        }        
    }
}
