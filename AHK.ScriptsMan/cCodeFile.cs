using System;
using System.Windows.Forms;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    public class cCodeFile : cFile
    {
        public cCodeFile(ref XPathNavigator xmlnav, string xpath, string datafile) : base(ref xmlnav, xpath, datafile)
        {
            this.Type = ResourceType.code;
            this.language = Guid.Parse(xmlnav.SelectSingleNode(xpath + "/@guid").Value);
            try
            {
                this.CompilationPath = xmlnav.SelectSingleNode(xpath + "/@compilation-path").Value;
            }
            catch
            {
            }

        }

        #region cCodeFile properties

        /// <summary>
        /// contains the languages to which the file is compatible
        /// </summary>
        public Guid language { get; protected set; }

        /// <summary>
        /// the path to save the files if it is compiled.
        /// </summary>
        public string CompilationPath { get; protected set; }

        #endregion
    }
}
