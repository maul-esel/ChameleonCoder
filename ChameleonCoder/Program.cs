using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.XPath;

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
                XPathNavigator xmlnav = new System.Xml.XPath.XPathDocument(file).CreateNavigator();
                AddResource(ref xmlnav, file, "/resource", nodes);
            }
            foreach (ResourceLink link in links)
            {
                link.UpdateLink();
            }
        }

        private static void AddResource(ref XPathNavigator xmlnav, string file, string xpath, TreeNodeCollection parentNodes)
        {
            cResource resource;
            int i;
            ResourceType type = (ResourceType)xmlnav.SelectSingleNode(xpath + "/@data-type").ValueAsInt;
            
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
            foreach (XPathNavigator xmlnav2 in xmlnav.Select(xpath + "/attach"))
            {
                i++;
                AddResource(ref xmlnav, file, xpath + "/attach[" + i + "]", resource.Node.Nodes);
            }

            i = 0;
            foreach (XPathNavigator xmlnav2 in xmlnav.Select(xpath + "/link"))
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

            ResourceList.GetInstance(node.GetHashCode()).Open();

            Program.Gui.listView2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            Program.Gui.panel1.Hide();
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
