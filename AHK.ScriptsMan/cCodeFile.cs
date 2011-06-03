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

            int i = 0;
            foreach (XPathNavigator node in xmlnav.Select(xpath + "/languages/lang"))
            {
                i++;
                languages[i] = Guid.Parse(node.SelectSingleNode("/@guid").Value);
            }
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
        public Guid[] languages { get; protected set; }

        /// <summary>
        /// the path to save the files if it is compiled.
        /// </summary>
        public string CompilationPath { get; protected set; }

        #endregion
    }
}
