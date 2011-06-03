using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    /// <summary>
    /// represents a library resource
    /// </summary>
    public class cLibrary : cCodeFile
    {
        public cLibrary(ref XPathNavigator xmlnav, string xpath, string datafile) : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.library;
            
            this.Author = xmlnav.SelectSingleNode(xpath + "/@author").Value;
            this.License = xmlnav.SelectSingleNode(xpath + "/@license").Value;
            this.Version = xmlnav.SelectSingleNode(xpath + "/@version").Value;
        }

        #region cLibrary properties

        public string Author { get; protected set; }

        public string License { get; protected set; }

        public string Version { get; protected set; }

        #endregion

               
    }
}
