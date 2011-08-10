﻿using System;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Interaction;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
{
    /// <summary>
    /// a class representing a resource that serves as link to another resource
    /// inherits from ResourceBase
    /// </summary>
    [CCPlugin]
    public class LinkResource : ResourceBase, IResolvable
    {
        /// <summary>
        /// creates a new instance of the LinkResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
        }

        #region IResource

        public override ImageSource Icon
        {
            get
            {
                IResource resource = Resolve();
                if (resource != null)
                    return resource.Icon;
                return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/link.png")).GetAsFrozen() as ImageSource;
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

        public bool shouldResolve { get { return true; } }

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

        [Resources.ResourceProperty("nameof_DestinationName", Resources.ResourcePropertyGroup.ThisClass, IsReadOnly = true, IsReferenceName = true)]
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
