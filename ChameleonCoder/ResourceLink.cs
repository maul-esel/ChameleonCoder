using System;
using System.Windows.Forms;
using System.Xml;

namespace ChameleonCoder
{
    internal sealed class ResourceLink
    {
        internal ResourceLink(ref XmlDocument xmlnav, string xpath, string datafile)
        {
            this.Node = new TreeNode();
            this.Node.NodeFont = new System.Drawing.Font("Cambria", 10, System.Drawing.FontStyle.Italic);
            
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.XML = xmlnav;
            this.XPath = xpath;
            ResourceList.AddLink(this.Node.GetHashCode(), this.GUID);
        }

        internal void UpdateLink()
        {
            this.Node.Text = this.Name = this.Resolve().Name;
            this.Type = this.Resolve().Type;
            this.Node.SelectedImageIndex = this.Node.ImageIndex = this.Resolve().Node.ImageIndex;
            this.Node.StateImageIndex = this.Resolve().Node.StateImageIndex;
        }

        #region properties
        
        internal string Name { get; private set; }

        internal Guid GUID { get; private set; }

        internal ResourceType Type { get; private set; }

        internal TreeNode Node { get; private set; }

        internal Guid Parent { get; private set; }

        internal XmlDocument XML { get; private set; }

        internal string XPath { get; private set; }

        #endregion

        internal cResource Resolve()
        {
            return ResourceList.GetInstance(this.GUID);
        }
    }
}
