using System;
using System.Windows.Forms;

namespace ChameleonCoder
{
    internal sealed partial class MainWin : Form
    {
        internal ListViewGroup[] groups { get; private set; }

        internal MainWin()
        {
            InitializeComponent();
            
            this.listView1.Columns.Add(string.Empty);
            this.listView1.Columns.Add(string.Empty);
            this.listView1.View = View.Details;
            this.listView1.Font = new System.Drawing.Font(this.listView1.Font.FontFamily, 10);
            this.listView1.HeaderStyle = ColumnHeaderStyle.None;

            this.listView1.ShowGroups = true;
            this.groups = new ListViewGroup[5];
            this.groups[0] = this.listView1.Groups.Add("files", string.Empty);
            this.groups[1] = this.listView1.Groups.Add("codes", string.Empty);
            this.groups[2] = this.listView1.Groups.Add("libraries", string.Empty);
            this.groups[3] = this.listView1.Groups.Add("projects", string.Empty);
            this.groups[4] = this.listView1.Groups.Add("tasks", string.Empty);
            
            this.TreeView.PathSeparator = "\\";
            this.TreeView.FullRowSelect = true;
            this.TreeView.ShowLines = true;
            this.TreeView.ShowPlusMinus = false;
            this.TreeView.ShowRootLines = true;
            this.TreeView.StateImageList = this.priorityIL;
            this.TreeView.Font = new System.Drawing.Font(this.listView1.Font.FontFamily, 10);
            this.TreeView.ItemHeight = 27;

            this.TreeView.Click += new EventHandler(Program.TreeView_Click);

            this.UpdateLanguage();
        }

        internal void UpdateLanguage()
        {
            this.toolbutton0_0.Text = Localization.get_string("Item_Home");
            this.toolbutton0_1.Text = Localization.get_string("Item_Options");
            this.toolbutton0_2.Text = Localization.get_string("Item_About");
            this.toolbutton0_3.Text = Localization.get_string("Item_Help");
            this.toolbutton0_4.Text = Localization.get_string("Item_Restart");
            this.toolbutton0_0.Click += new EventHandler(GoHome);
            this.groups[0].Header = Localization.get_string("ResourceType_File_pl");
            this.groups[1].Header = Localization.get_string("ResourceType_Code_pl");
            this.groups[2].Header = Localization.get_string("ResourceType_Library_pl");
            this.groups[3].Header = Localization.get_string("ResourceType_Project_pl");
            this.groups[4].Header = Localization.get_string("ResourceType_Task_pl");
        }

        internal void GoHome(object sender, EventArgs e)
        {
            this.panel2.Hide();
            this.panel3.Hide();
            this.panel1.Show();
        }
    }
}
