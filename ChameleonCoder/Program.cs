using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Xml;

namespace ChameleonCoder
{
    internal sealed class Program
    {
        /// <summary>
        /// contains the window object as public property
        /// </summary>
        public static MainWin Gui
        {
            get;
            private set;
        }

        internal static Form Selector
        {
            get;
            set;
        }

        internal static System.Collections.ArrayList links { get; private set; }

        
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

            try
            {
                Plugins.PluginManager.LoadPlugins();
            }
            catch (Exception e) { MessageBox.Show(e.Message + "\n\n" + e.StackTrace); }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Gui = new MainWin();
            ListData();
 
            Gui.FormClosed += new FormClosedEventHandler(Gui_FormClosed);
            Gui.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            Gui.TreeView.ExpandAll();
            Gui.TreeView.Sort();
            Gui.TreeView.TreeViewNodeSorter = new TreeNodeSorter();
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
            links = new System.Collections.ArrayList();

            TreeNodeCollection nodes = Gui.TreeView.Nodes;
            string[] files = Directory.GetFiles(Application.StartupPath + "\\#Data", "*.xml");

            foreach (string file in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                AddResource(ref doc, file, "/resource", nodes);
            }
            foreach (ResourceLink link in links)
            {
                link.UpdateLink();
            }
        }

        private static void AddResource(ref XmlDocument xmlnav, string file, string xpath, TreeNodeCollection parentNodes)
        {
            cResource resource;
            int i;
            ResourceType type = (ResourceType)xmlnav.CreateNavigator().SelectSingleNode(xpath + "/@data-type").ValueAsInt;
            
            switch (type)
                {
                    case ResourceType.file:
                        resource = new cFile(ref xmlnav, xpath, file);
                        i = 0; break;
                    case ResourceType.code:
                        resource = new cCodeFile(ref xmlnav, xpath, file);
                        i = 1; break;
                    case ResourceType.library:
                        resource = new cLibrary(ref xmlnav, xpath, file);
                        i = 2; break;
                    case ResourceType.project:
                        resource = new cProject(ref xmlnav, xpath, file);
                        i = 3; break;
                    case ResourceType.task:
                        resource = new cTask(ref xmlnav, xpath, file);
                        i = 4; break;
                    default:
                        throw new Exception("parsing error in file " + file + ".\ncase:" + type);
            }
            Gui.listView1.Items.Add(resource.Item);
            Gui.listView1.Groups[i].Items.Add(resource.Item);
            ResourceList.Add(resource.Node.GetHashCode(), resource.GUID, resource);
            resource.Node.SelectedImageIndex = resource.Node.ImageIndex;
            parentNodes.Add(resource.Node);
           
            i = 0;
            foreach (XmlNode xmlnav2 in xmlnav.SelectNodes(xpath + "/attach"))
            {
                i++;
                AddResource(ref xmlnav, file, xpath + "/attach[" + i + "]", resource.Node.Nodes);
            }

            i = 0;
            foreach (XmlNode xmlnav2 in xmlnav.SelectNodes(xpath + "/link"))
            {
                i++;
                ResourceLink link = new ResourceLink(ref xmlnav, xpath + "/link[" + i + "]", file);
                links.Add(link);
                resource.Node.Nodes.Add(link.Node);
            }
        }

        internal static void TreeView_Click(object sender, EventArgs e)
        {
            TreeView tree = (TreeView)sender;
            MouseEventArgs ev = (MouseEventArgs)e;
            TreeNode node = tree.GetNodeAt(ev.X, ev.Y);

            cResource old = ResourceList.GetActiveInstance();
            if (old != null)
            {
                old.Save(); // save old opened instance
            }

            ResourceList.SetActiveInstance(node.GetHashCode()); // set new opened instance

            ResourceList.GetActiveInstance().Open(); // open the new instance

            Program.Gui.listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent); // resize columns

            Program.Gui.panel1.Hide(); // switch to resource view panel
            Program.Gui.panel2.Hide();
            Program.Gui.panel3.Show();
        }

        internal static void CreateProject()
        {
            
        }

        internal static void CreateFile()
        {
            
        }

        internal static void CreateLibrary()
        {
            
        }

        internal static void CreateTask()
        {
            
        }

        internal static void OnLanguageChanged(string newlang)
        {
            
        }

        internal static bool FilesAreMissing()
        {
            return !(File.Exists(Application.StartupPath + "\\ScintillaNet.dll")
                && File.Exists(Application.StartupPath + "\\SciLexer.dll"));
        }    
    }
}
