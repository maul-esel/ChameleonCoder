using System;
using System.Windows.Forms;
using System.Xml.XPath;

namespace ChameleonCoder
{
    internal sealed class ResourceLink
    {
        internal ResourceLink(ref XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.Node = new TreeNode();
            this.GUID = new Guid(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            this.XML = xmlnav;
            ResourceList.AddLink(this.Node.GetHashCode(), this.GUID);
        }

        internal void UpdateLink()
        {
            this.Node.Text = this.Name = this.Resolve().Name;
            this.Type = this.Resolve().Type;

            switch (this.Type)
            {
                case ResourceType.file: this.Node.SelectedImageIndex = this.Node.ImageIndex = 0; break;
                case ResourceType.code: this.Node.SelectedImageIndex = this.Node.ImageIndex = 1; break;
                case ResourceType.library: this.Node.SelectedImageIndex = this.Node.ImageIndex = 2; break;
                case ResourceType.project: this.Node.SelectedImageIndex = this.Node.ImageIndex = 3; break;
                case ResourceType.task: this.Node.SelectedImageIndex = this.Node.ImageIndex = 4; break;
            }
        }

        #region properties
        
        internal string Name { get; private set; }

        internal Guid GUID { get; private set; }

        internal ResourceType Type { get; private set; }

        internal TreeNode Node { get; private set; }

        internal Guid Parent { get; private set; }

        internal XPathNavigator XML { get; private set; }

        #endregion

        internal IResource Resolve()
        {
            return ResourceList.GetInstance(this.GUID);
        }
    }
}
