using System;
using System.Windows.Media;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// a class representing a resource that serves as link to another resource
    /// </summary>
    public class LinkResource : ResourceBase, IResolvable
    {
        #region IResource

        public override ImageSource Icon
        {
            get
            {
                IResource resource = Resolve();
                if (resource != null)
                    return resource.Icon;
                return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/link.png")).GetAsFrozen() as ImageSource;
            }
        }

        /// <summary>
        /// the name of the link which can either be an own, independant name or the destination's name
        /// </summary>
        public override string Name
        {
            get
            {
                string result = base.Name;
                if (string.IsNullOrWhiteSpace(result))
                {
                    IResource target = Resolve();
                    if (target != null)
                        return target.Name;
                }
                return result;
            }
            set
            {
                base.Name = value;
            }
        }

        public override string Description
        {
            get
            {
                string result = base.Description;
                if (string.IsNullOrWhiteSpace(result))
                {
                    IResource target = Resolve();
                    if (target != null)
                        return target.Description;
                }
                return result;
            }
            set
            {
                base.Description = value;
            }
        }

        public override ImageSource SpecialVisualProperty
        {
            get
            {
                IResource target = Resolve();
                if (target != null)
                    return target.SpecialVisualProperty;
                return null;
            }
        }

        #endregion

        #region IResolvable

        /// <summary>
        /// gets the destination instance
        /// </summary>
        /// <returns>the Resource object the link points to</returns>
        public IResource Resolve()
        {
            return InformationProvider.GetResourceInstance(Destination);
        }

        public bool ShouldResolve { get { return true; } }

        #endregion

        /// <summary>
        /// the GUID of the resource the link points to
        /// </summary>
        public Guid Destination
        {
            get
            {
                Guid dest;
                string guid = Xml.GetAttribute("destination");
                if (!Guid.TryParse(guid, out dest))
                    return Guid.Empty;
                return dest;
            }
            protected set
            {
                Xml.SetAttribute("destination", value.ToString());
                OnPropertyChanged("Destination");
            }
        }

        [ResourceProperty("nameof_DestinationName", ResourcePropertyGroup.ThisClass, IsReadOnly = true, IsReferenceName = true)]
        public string DestinationName
        {
            get
            {
                IResource resource = InformationProvider.GetResourceInstance(Destination);
                if (resource != null)
                    return resource.Name;
                return string.Empty;
            }
        }

        public string nameof_DestinationName
        {
            get
            {
                return Properties.Resources.Info_Destination;
            }
        }

        internal const string Alias = "link";
    }
}
