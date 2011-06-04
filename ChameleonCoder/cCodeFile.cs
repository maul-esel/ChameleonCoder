using System;
using System.Windows.Forms;
using System.Xml.XPath;

namespace ChameleonCoder
{
    internal class cCodeFile : cFile
    {
        internal cCodeFile(ref XPathNavigator xmlnav, string xpath, string datafile) : base(ref xmlnav, xpath, datafile)
        {
            this.Node.ImageIndex = 1;
            this.Type = ResourceType.code;
            this.language = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);

            try { this.CompilationPath = xmlnav.SelectSingleNode(xpath + "/@compilation-path").Value; }
            catch { }
        }

        #region cCodeFile properties

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        internal Guid language { get; set; }

        /// <summary>
        /// the path to save the file if it is compiled.
        /// </summary>
        internal string CompilationPath { get; set; }

        #endregion
    }
}
