using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.XPath;
using AHKScriptsMan.Window;

namespace AHKScriptsMan
{
    public static class Program
    {
        /// <summary>
        /// contains the window object as public property
        /// </summary>
        public static MainWin Gui
        {
            get;
            set;
        }

        
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            Localization.UpdateLanguage(Properties.Settings.Default.Language);
            if (FilesAreMissing())
            {
                MessageBox.Show("required files are missing!", "AHKScriptsMan error:", MessageBoxButtons.OK);
                Application.Exit();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Gui = new MainWin();
            ListData();
            Gui.FormClosed += new FormClosedEventHandler(Gui_FormClosed);
            Gui.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Application.Run(Gui);            
        }

        static void Gui_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// lists all resources
        /// </summary>
        private static void ListData()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "\\#Data", "*.xml");
            cFile res1; cCodeFile res2;  cLibrary res3; cProject res4; cTask res5;
            foreach (string file in files)
            {
#if DEBUG
                try
                {
#endif
                    XPathDocument xmldoc = new System.Xml.XPath.XPathDocument(file);
                    XPathNavigator xmlnav = xmldoc.CreateNavigator();

                    ResourceType type = (ResourceType)xmlnav.SelectSingleNode("/resource/@data-type").ValueAsInt;
                    
                    switch (type)
                    {
                        case ResourceType.file:
                            res1 = new cFile(ref xmlnav, "/resource", file);
                            Gui.TreeView.Nodes.Add(res1.Node);
                            Gui.listView1.Items.Add(res1.Item);
                            Gui.groups[0].Items.Add(res1.Item);
                            // problem: handle immer 0 (noch nicht erstellt :( )
                            ResourceList.Add(res1.Node.Handle, res1.GUID, res1.Type, res1);
                            break;
                        case ResourceType.code:
                            res2 = new cCodeFile(ref xmlnav, "/resource", file);
                            Gui.TreeView.Nodes.Add(res2.Node);
                            Gui.listView1.Items.Add(res2.Item);
                            Gui.groups[1].Items.Add(res2.Item);
                            ResourceList.Add(res2.Node.Handle, res2.GUID, res2.Type, res2);
                            break;
                        case ResourceType.library:
                            res3 = new cLibrary(ref xmlnav, "/resource", file);
                            Gui.TreeView.Nodes.Add(res3.Node);
                            Gui.listView1.Items.Add(res3.Item);
                            Gui.groups[2].Items.Add(res3.Item);
                            ResourceList.Add(res3.Node.Handle, res3.GUID, res3.Type, res3);
                            break;
                        case ResourceType.project:
                            res4 = new cProject(ref xmlnav, "/resource", file);
                            Gui.TreeView.Nodes.Add(res4.Node);
                            Gui.listView1.Items.Add(res4.Item);
                            Gui.groups[3].Items.Add(res4.Item);
                            ResourceList.Add(res4.Node.Handle, res4.GUID, res4.Type, res4);
                            break;
                        case ResourceType.task:
                            res5 = new cTask(ref xmlnav, "/resource", file);
                            Gui.TreeView.Nodes.Add(res5.Node);
                            Gui.listView1.Items.Add(res5.Item);
                            Gui.groups[4].Items.Add(res5.Item);
                            ResourceList.Add(res5.Node.Handle, res5.GUID, res5.Type, res5);
                            break;
                        default:
                            MessageBox.Show("parsing error in file " + file + ".\ncase:" + type, "AHK.ScriptsMan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // TODO: check exceptions
                            continue;
                    }
#if DEBUG
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
                }
#endif
                                
            }

            
        }

        public static void TreeView_Click(object sender, EventArgs e)
        {
            
        }

        public static void Window_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        public static void CreateProject()
        {
            
        }

        public static void CreateFile()
        {
            
        }

        public static void CreateLibrary()
        {
            
        }

        public static void CreateTask()
        {
            
        }

        public static void OnLanguageChanged(string newlang)
        {
            
        }

        public static bool FilesAreMissing()
        {
            return !(File.Exists(Application.StartupPath + "\\#Extern\\ScintillaNet.dll")
                && File.Exists(Application.StartupPath + "\\#Extern\\SciLexer.dll"));
        }
    
    }
}
