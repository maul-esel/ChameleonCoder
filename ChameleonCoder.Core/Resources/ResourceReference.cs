using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Files;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// represents a reference to another resource
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.None), Guid("207E8CE3-61DB-431D-A9E5-A24A910DDC15")]
    public sealed class ResourceReference : IResourceReference
    {
        /// <summary>
        /// creates a new instance of the ResourceReference class
        /// </summary>
        /// <param name="data">the XmlElement representing the reference</param>
        public ResourceReference(IObservableStringDictionary data, IDataFile file)
        {
            Attributes = data;
            File = file;
        }

        public IDataFile File
        {
            get;
            private set;
        }

        /// <summary>
        /// implements IComponent.Icon
        /// </summary>
        public ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.UI;component/Images/reference.png")).GetAsFrozen() as ImageSource; }
        }

        /// <summary>
        /// implements IComponent.Identifier
        /// </summary>
        public Guid Identifier
        {
            get { return Guid.Parse(Attributes["id"]); }
        }

        /// <summary>
        /// implements IComponent.Name
        /// </summary>
        public string Name
        {
            get
            {
                return Attributes["name"];
            }
        }

        /// <summary>
        /// gets the target's special visual property
        /// </summary>
        public ImageSource SpecialVisualProperty
        {
            get
            {
                IResource res = Resolve();
                return res != null ? res.SpecialVisualProperty : null;
            }
        }

        /// <summary>
        /// contains the Identifier of the target resource
        /// </summary>
        private Guid Target
        {
            get { return Guid.Parse(Attributes["target"]); }
        }

        public IObservableStringDictionary Attributes
        {
            get;
            private set;
        }

        /// <summary>
        /// gets the resource referenced by this instance
        /// </summary>
        /// <returns>the IResource instance for the resource</returns>
        public IResource Resolve()
        {
            return File.App.ResourceMan.GetResource(Target);
        }        
    }
}
