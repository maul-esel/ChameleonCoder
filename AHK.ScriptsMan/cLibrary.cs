using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Xml.XPath;

namespace ChameleonCoder
{
    /// <summary>
    /// represents a library resource
    /// </summary>
    internal sealed class cLibrary : cCodeFile
    {
        internal cLibrary(ref XPathNavigator xmlnav, string xpath, string datafile) : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.library;

            this.Node.ImageIndex = 2;
            this.Author = xmlnav.SelectSingleNode(xpath + "/@author").Value;
            this.License = xmlnav.SelectSingleNode(xpath + "/@license").Value;
            this.Version = xmlnav.SelectSingleNode(xpath + "/@version").Value;
        }

        #region cLibrary properties

        internal string Author { get; private set; }

        internal string License { get; private set; }

        internal string Version { get; private set; }

        #endregion

               
    }
}
