using System;
using System.Windows.Forms;
using System.Xml.XPath;

namespace AHKScriptsMan
{
    public class cCodeFile : cFile, IResource
    {
        public cCodeFile(XPathNavigator xmlnav, string xpath, string datafile) : base(xmlnav, xpath, datafile)
        {
            this.Item = new ListViewItem(new string[] { this.Name, this.Description, this.Type.ToString() });
            this.Compatible_AHKB = xmlnav.SelectSingleNode(xpath + "/@compatible_AHKB").ValueAsBoolean;
            this.Compatible_AHKL = xmlnav.SelectSingleNode(xpath + "/@compatible_AHKL").ValueAsBoolean;
            this.Compatible_AHKI = xmlnav.SelectSingleNode(xpath + "/@compatible_AHKI").ValueAsBoolean;
            this.Compatible_AHK2 = xmlnav.SelectSingleNode(xpath + "/@compatible_AHK2").ValueAsBoolean;
            this.Type = ResourceType.code;
        }

        #region cCodeFile properties

        /// <summary>
        /// defines whether the code is compatible to AutoHotkey (basic).
        /// </summary>
        bool Compatible_AHKB { get; set; }

        /// <summary>
        /// defines whether the code is compatible to AutoHotkey_L.
        /// </summary>
        bool Compatible_AHKL { get; set; }

        /// <summary>
        /// defines whether the code is compatible to IronAHK.
        /// </summary>
        bool Compatible_AHKI { get; set; }

        /// <summary>
        /// defines whether the code is compatible to AutoHotkey v2.
        /// </summary>
        bool Compatible_AHK2 { get; set; }

        /// <summary>
        /// the path to save the files if it is compiled.
        /// </summary>
        string CompilationPath { get; set; }

        #endregion
    }
}
