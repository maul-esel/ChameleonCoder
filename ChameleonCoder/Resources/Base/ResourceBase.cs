using System;
using System.Windows.Controls;
using System.Xml;
using System.Windows.Media;
using ChameleonCoder.Resources.Collections;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Base
{
    /// <summary>
    /// an abstract class for resources
    /// </summary>
    public abstract partial class ResourceBase : IResource
    {
        /// <summary>
        /// serves as base constructor for inherited classes and sets general properties
        /// </summary>
        /// <param name="node">the XmlNode that contains the resource</param>
        public ResourceBase(XmlNode node)
        {
            this.XMLNode = node;

            this.GUID = new Guid(node.Attributes["guid"].Value);

            this.MetaData = new MetadataCollection();

            int i = 0;
            Metadata data;
            try
            {
                foreach (XmlNode meta in node.SelectNodes("/metadata")) //this.XML.SelectNodes(this.XPath + "/metadata"))
                {
                    i++;
                    System.Windows.MessageBox.Show(meta.Attributes["name"] + " = " + meta.Value);
                    //this.MetaData.Add(data = new Metadata(ref xml, this.XPath + "/metadata[" + i + "]"));
                }
            }
            catch { }

            this.children = new Collections.ResourceCollection();
        }

        public ResourceBase() { }

        XmlNode XMLNode;

        #region IResource

        public abstract ImageSource Icon { get; }

        public abstract StaticInfo Info { get; }

        /// <summary>
        /// saves the information changed through the UI to the current instance and its XML representation
        /// </summary>
        public virtual void Save()
        {
            this.XML.SelectSingleNode(this.XPath + "/@data-type").Value = ((int)this.Type).ToString();

            foreach (Metadata data in this.MetaData)
            {
                data.Save(); // changes through the UI should be saved when they occur or through binding
            }

            System.IO.File.WriteAllText(this.DataFile, this.XMLNode.OwnerDocument.DocumentElement.OuterXml);
        }

        #endregion 

        /// <summary>
        /// the file that contains the resources definition
        /// </summary>
        internal string DataFile { get; private set; }

        /// <summary>
        /// the display name of the resource
        /// </summary>
        public string Name
        {
            get
            {
                try
                {
                    return this.XMLNode.Attributes["name"].Value;
                }
                catch (NullReferenceException) { return string.Empty; }
            }
            protected internal set { this.XMLNode.Attributes["name"].Value = value; }
        }

        /// <summary>
        /// the GUID that uniquely identifies the resource
        /// </summary>
        public Guid GUID { get; private set; }

        /// <summary>
        /// defines the resource's type
        /// </summary>
        [Obsolete()]
        public ResourceType Type { get; protected internal set; }

        /// <summary>
        /// the XPath to the resource's definition in the datafile
        /// </summary>
        [Obsolete()]
        internal string XPath { get; private set; }

        /// <summary>
        /// a short description of the resource
        /// </summary>
        public string Description
        {
            get
            {
                try
                {
                    return this.XMLNode.Attributes["description"].Value;
                }
                catch (NullReferenceException) { return null; }
            }
            protected internal set { this.XMLNode.Attributes["description"].Value = value; }
        }

        /// <summary>
        /// the associated metadata as Metadata class instances
        /// </summary>
        public MetadataCollection MetaData { get; protected internal set; }

        /// <summary>
        /// any notes related to the resource
        /// </summary>
        public string Notes
        {
            get
            {
                try { return this.XMLNode.Attributes["notes"].Value; }
                catch (NullReferenceException) { return string.Empty; }
            }
            set { this.XMLNode.Attributes["notes"].Value = value; }
        }

        /// <summary>
        /// the XmlDocument used to navigat through the resource's contents
        /// </summary>
        protected internal XmlDocument XML { get; set; }

        /// <summary>
        /// a ResourceCollection containing the children resources
        /// </summary>
        public Resources.Collections.ResourceCollection children { get; protected internal set; }


        #region methods

        /// <summary>
        /// opens a resource
        /// </summary>
        public virtual void Open()
        {
            App.Gui.PropertyGrid.Items.Clear();

            // maybe this could be done with binding and several data templates.
            // Currently it doesn't work because the contents are not added (see comments).
            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Name"), this.Name }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("ResourceType"), ToString(this.Type) }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Tree"), "/" + this.Node.FullPath }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("Description"), this.Description }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("DataFile"), this.DataFile }));

            App.Gui.PropertyGrid.Items.Add(new ListViewItem()); //new string[] { Localization.get_string("GUID"), this.GUID.ToString() }));
        }

        /// <summary>
        /// packages a resource into a zip file,
        /// including all attached or linked resources,
        /// all data files,
        /// any file a resource points to (FileResource, CodeResource, LibraryResource)
        /// </summary>
        public virtual void Package()
        {

        }

        

        /// <summary>
        /// adds a metadata element, given any changes through the UI
        /// it directly manipulates
        /// 1) the XmlDocument
        /// 2) the MetadataCollection
        /// </summary>
        internal virtual void AddMetadata()
        {
            // ...

            this.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void DeleteMetadata()
        {

        }

        /// <summary>
        /// attaches a copy of the resource to another resource
        /// to do so, a new GUID is created.
        /// a parameter or user input define where it goes:
        /// the original resource or the new copy
        /// </summary>
        internal virtual void AttachResource() // todo: parameter to define where the guid goes
        {

        }

        /// <summary>
        /// receives a resource that should be attached,
        /// adds it into the tree (-view), the children collection and the XmlDocument
        /// </summary>
        internal virtual void ReceiveAttach()
        {

        }

        /// <summary>
        /// links a resource to another resource
        /// </summary>
        internal virtual void LinkResource()
        {

        }

        /// <summary>
        /// receives a resource link
        /// </summary>
        internal virtual void ReceiveLink()
        {

        }

        /// <summary>
        /// moves a resource to another resource
        /// by first copying and then deleting it.
        /// When it is copied, the copy should receive the old GUID
        /// </summary>
        internal virtual void Move()
        {

        }

        /// <summary>
        /// deletes a resource by removing it from the tree (-view), any Collections and the XmlDocument
        /// </summary>
        internal virtual void Delete()
        {
            
        }

        /// <summary>
        /// overrides object.ToString()
        /// </summary>
        /// <returns>the name of the resource</returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
