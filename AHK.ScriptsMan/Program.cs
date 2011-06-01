using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace AHKScriptsMan
{
    static class Program
    {
        /// <summary>
        /// contains the window object as public property
        /// </summary>
        public static WindowsFormsApplication1.MainWin Window
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
            Application.Run(Window = new WindowsFormsApplication1.MainWin());
            
            FinishGui();
            ListData();
            Window.Show();
        }

        /// <summary>
        /// lists all resources
        /// </summary>
        private static void ListData()
        {
            System.Xml.XPath.XPathNodeIterator List = new System.Xml.XPath.XPathDocument(Application.StartupPath + "\\Settings.xml").CreateNavigator().Select("/settings/tasks/task");
            TreeNode TaskNode = Window.TreeView.Nodes.Add("TaskNode", "tasks", "icon1");
            foreach (System.Xml.XPath.XPathNavigator xmlnav in List)
            {
                string name = xmlnav.SelectSingleNode("/@name").ToString();
                string description = xmlnav.ToString();
                ListViewItem item = new ListViewItem( new string[] {name, description, "(none)"} );
                //Window.listView1.Items.Add(item);
            }
            
            string[] files = Directory.GetFiles(Application.StartupPath + "\\#Data", "*.xml");
            TreeNode ResourceNode = Window.TreeView.Nodes.Add("ResourceNode", "resources", "icon2");
            foreach (string file in files)
            {
                System.Xml.XPath.XPathDocument xmldoc = new System.Xml.XPath.XPathDocument(file);
                System.Xml.XPath.XPathNavigator xmlnav = xmldoc.CreateNavigator();
                
                switch (xmlnav.SelectSingleNode("/resource/@name").ToString())
                {
                    case "file":
                        Data.cFile res1 = new Data.cFile();
                        res1.List(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res1.Node);
                        break;
                    case "library":
                        Data.cLibrary res2 = new Data.cLibrary();
                        res2.List(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res2.Node);
                        break;
                    case "project":
                        Data.cProject res3 = new Data.cProject();
                        res3.List(xmlnav, "/resource", IntPtr.Zero, file);
                        ResourceNode.Nodes.Add(res3.Node);
                        break;
                    default:
                        MessageBox.Show("parsing error in file " + file + ".", "AHKScriptsMan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // TODO: check exceptions
                        continue;
                }
                Window.TreeView.Click += new EventHandler(TreeView_Click);
                
            }
        }

        static void TreeView_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// finishs the GUI build process
        /// </summary>
        private static void FinishGui()
        {
            Window.TreeView.ImageList = AHKScriptsMan.Window.DataProvider.GetImageList(9, 5);
            Window.DragDrop += new DragEventHandler(Window_DragDrop);

            Window.TreeView.PathSeparator = "\\";

            

        }

        static void Window_DragDrop(object sender, DragEventArgs e)
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

        }

        

        public static bool FilesAreMissing()
        {
            return !(File.Exists(Application.StartupPath + "\\Settings.xml")
                && File.Exists(Application.StartupPath + "\\#Extern\\ScintillaNet.dll")
                && File.Exists(Application.StartupPath + "\\#Extern\\SciLexer.dll"));
        }
    
    }
}
