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
            IResource resource;
            int i;
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
                            resource = new cFile(ref xmlnav, "/resource", file);
                            i = 0; break;
                        case ResourceType.code:
                            resource = new cCodeFile(ref xmlnav, "/resource", file);
                            i = 1; break;
                        case ResourceType.library:
                            resource = new cLibrary(ref xmlnav, "/resource", file);
                            i = 2; break;
                        case ResourceType.project:
                            resource = new cProject(ref xmlnav, "/resource", file);
                            i = 3; break;
                        case ResourceType.task:
                            resource = new cTask(ref xmlnav, "/resource", file);
                            i = 4;  break;
                        default:
                            MessageBox.Show("parsing error in file " + file + ".\ncase:" + type, "AHK.ScriptsMan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // TODO: check exceptions
                            continue;
                    }
                    Gui.TreeView.Nodes.Add(resource.Node);
                    Gui.listView1.Items.Add(resource.Item);
                    Gui.groups[i].Items.Add(resource.Item);
                    ResourceList.Add(resource.Node.GetHashCode(), resource.GUID, resource.Type, resource);
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
