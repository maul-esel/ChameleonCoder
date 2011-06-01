using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.XPath;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using AHKScriptsMan.Window;

namespace AHKScriptsMan
{
    public static class Program
    {
        /// <summary>
        /// contains the window object as public property
        /// </summary>
        public static WindowsFormsApplication1.MainWin Gui
        {
            get;
            set;
        }
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (FilesAreMissing())
            {
                MessageBox.Show("required files are missing!", "AHKScriptsMan error:", MessageBoxButtons.OK);
                Application.Exit();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Gui = new WindowsFormsApplication1.MainWin());

            Gui.FinishGui();
            ListData();
            Gui.Show();
        }

        /// <summary>
        /// lists all resources
        /// </summary>
        private static void ListData()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "\\#Data", "*.xml");
            TreeNode ResourceNode = Gui.TreeView.Nodes.Add("ResourceNode", "resources", "icon2");
            foreach (string file in files)
            {
                XPathDocument xmldoc = new System.Xml.XPath.XPathDocument(file);
                XPathNavigator xmlnav = xmldoc.CreateNavigator();
                                         
                switch (xmlnav.SelectSingleNode("/resource/@name").ToString())
                {
                    case "file": Data.cFile res1 = new Data.cFile(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res1.Node);
                        break;
                    case "library":
                        Data.cLibrary res2 = new Data.cLibrary(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res2.Node); break;
                    case "project":
                        Data.cProject res3 = new Data.cProject(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res3.Node); break;
                    case "task":
                        Data.cTask res4 = new Data.cTask(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res4.Node); break;
                    default:
                        MessageBox.Show("parsing error in file " + file + ".", "AHK.ScriptsMan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // TODO: check exceptions
                        continue;
                }
                                
            }
            Gui.TreeView.Click += new EventHandler(TreeView_Click);
        }

        static void TreeView_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void Window_DragDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static void CreateProject()
        {
            throw new NotImplementedException();
        }

        public static void CreateFile()
        {
            throw new NotImplementedException();
        }

        public static void CreateLibrary()
        {
            throw new NotImplementedException();
        }

        public static void CreateTask()
        {
            throw new NotImplementedException();
        }

        public static void OnLanguageChanged(string newlang)
        {
            throw new NotImplementedException();
        }

        public static bool FilesAreMissing()
        {
            return !(File.Exists(Application.StartupPath + "\\Settings.xml")
                && File.Exists(Application.StartupPath + "\\#Extern\\ScintillaNet.dll")
                && File.Exists(Application.StartupPath + "\\#Extern\\SciLexer.dll"));
        }
    
    }
}
