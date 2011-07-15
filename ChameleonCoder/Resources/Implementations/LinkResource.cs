using System;
using System.Windows.Media;
using System.Xml;
using System.Collections.Generic;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// a class representing a resource that serves as link to another resource
    /// inherits from ResourceBase
    /// </summary>
    public class LinkResource : ResourceBase, IResolvable
    {
        /// <summary>
        /// creates a new instance of the LinkResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public LinkResource(XmlNode node)
            : base(node)
        {
        }

        #region IResource

        public override ImageSource Icon { get { return this.Resolve().Icon; } }

        /// <summary>
        /// the name of the link which can either be an own, independant name or the destination's name
        /// </summary>
        public override string Name
        {
            get
            {
                string result = string.Empty;

                try { result = base.Name; }
                catch (NullReferenceException) { }

                if (!string.IsNullOrWhiteSpace(result))
                    return result;

                try { result = this.Resolve().Name; }
                catch (NullReferenceException) { }

                return result;
            }
            protected set { base.Name = value; }
        }

        public override string Description
        {
            get
            {
                string result = string.Empty;
                try
                {
                    result = base.Description;
                }
                catch (NullReferenceException) { }
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                try
                {
                    result = this.Resolve().Description;
                }
                catch (NullReferenceException) { }
                return result;
            }
            protected set { base.Description = value; }
        }

        public override ImageSource SpecialVisualProperty
        {
            get
            {
                IResource resource = this;
                IResolvable link;

                while ((link = resource as IResolvable) != null && link.shouldResolve)
                    resource = link.Resolve();

                return resource.SpecialVisualProperty;
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
            return ResourceManager.FlatList.GetInstance(this.Destination);
        }

        public bool shouldResolve { get { return true; } }

        #endregion

        #region IEnumerable

        public override IEnumerator<PropertyDescription> GetEnumerator()
        {
            IEnumerator<PropertyDescription> baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            yield return new PropertyDescription("Destination", this.Destination.ToString("b"), "link");
        }

        #endregion

        /// <summary>
        /// the GUID of the resource the link points to
        /// </summary>
        public Guid Destination
        {
            get { return new Guid(this.Xml.Attributes["destination"].Value); }
            protected set
            {
                this.Xml.Attributes["destination"].Value = value.ToString();
                this.OnPropertyChanged("Destination");
            }
        }
    }
}
