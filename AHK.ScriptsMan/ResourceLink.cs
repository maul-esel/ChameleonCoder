using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    public class ResourceLink
    {
        public ResourceLink(XPathNavigator xmlnav, string xpath, string datafile)
        {
            this.Name = "";
        }

        #region properties
        string Name { get; set; }


        #endregion

        public void Resolve()
        {

        }
    }
}
