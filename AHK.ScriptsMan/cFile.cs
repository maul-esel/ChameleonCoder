using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AHKScriptsMan.Data
{
    /// <summary>
    /// represents a file resource
    /// </summary>
    public class cFile : cResource
    {
        public cFile(XPathNavigator xmlnav, string xpath, IntPtr parentID, string datafile)
        {
            this.Name = xmlnav.SelectSingleNode(xpath + "/@name").Value;
            if (ResourceList.HasKey(this.Name))
            {
                throw new Exception("duplicate resource name:" + this.Name + "\nresource type: file");
            }
            this.Type = ResourceType.file;
            this.ParentID = parentID;
            this.XML = xmlnav;
            this.XPath = xpath;
            this.DataFile = datafile;
            this.Node = new TreeNode(this.Name);
            ResourceList.Add((object)this);

            // add to LV
        }
    }
}
