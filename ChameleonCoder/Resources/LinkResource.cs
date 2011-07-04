using System;
using System.Xml;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// a class representing a resource that serves as link to another resource
    /// inherits from ResourceBase
    /// </summary>
    public sealed class LinkResource : ResourceBase, IResolvable
    {
        /// <summary>
        /// creates a new instance of the LinkResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the XPath in the XmlDocument to the resource's root element</param>
        /// <param name="datafile">the file that contains the definition</param>
        internal LinkResource(ref XmlDocument xml, string xpath, string datafile) : base(ref xml, xpath, datafile)
        {
            this.Type = ResourceType.link;
        }

        public LinkResource() { }

        #region IResource

        public override string Alias { get { return "link"; } }

        #endregion

        #region IResolvable

        /// <summary>
        /// gets the destination instance
        /// </summary>
        /// <returns>the Resource object the link points to</returns>
        public ResourceBase Resolve()
        {
            return ResourceManager.FlatList.GetInstance(this.Destination);
        }

        public bool shouldResolve { get { return true; } }

        #endregion

        /// <summary>
        /// the name of the link which can either be an own, independant name or the destination's name
        /// </summary>
        public new string Name
        {
            get
            {
                string result = string.Empty;
                try
                {
                    result = base.Name;
                }
                catch (NullReferenceException) { }
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                try
                {
                    result = this.Resolve().Name;
                }
                catch (NullReferenceException) { }
                return result;
            }
            private set { base.Name = value; }
        }

        public new string Description
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
            private set { base.Description = value; }
        }

        

        #region methods

        internal override void Open()
        {
            this.Resolve().Open();
        }

        internal override void Package()
        {
            this.Resolve().Package();
        }

        public override void Save()
        {
            base.Save();
            this.Resolve().Save();
        }

        #endregion

        

        /// <summary>
        /// the GUID of the resource the link points to
        /// </summary>
        [Obsolete("use Resolve() only")]
        public Guid Destination
        {
            get { return new Guid(this.XML.SelectSingleNode(this.XPath + "/@destination").Value); }
            private set { this.XML.SelectSingleNode(this.XPath + "/@destination").Value = value.ToString(); }
        }
    }
}
