using System;
using System.Windows.Forms;

namespace ChameleonCoder
{
    internal sealed partial class MainWin : Form
    {

        internal MainWin()
        {
            InitializeComponent();

            this.listView1.Columns.Add(string.Empty);
            this.listView1.Columns.Add(string.Empty);
            this.listView1.Columns.Add(string.Empty);
            this.listView1.View = View.Details;
            this.listView1.Font = new System.Drawing.Font(this.listView1.Font.FontFamily, 10);
            this.listView1.HeaderStyle = ColumnHeaderStyle.None;
            this.listView1.FullRowSelect = true;

            this.listView1.ShowGroups = true;
            this.listView1.Groups.Add("files", string.Empty);
            this.listView1.Groups.Add("codes", string.Empty);
            this.listView1.Groups.Add("libraries", string.Empty);
            this.listView1.Groups.Add("projects", string.Empty);
            this.listView1.Groups.Add("tasks", string.Empty);


            this.listView2.Columns.Add(string.Empty);
            this.listView2.Columns.Add(string.Empty);
            this.listView2.View = View.Details;
            this.listView2.Font = new System.Drawing.Font(this.listView1.Font.FontFamily, 10);
            this.listView2.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listView2.FullRowSelect = true;

            this.listView2.ShowGroups = true;
            this.listView2.Groups.Add("general", string.Empty);
            this.listView2.Groups.Add("specific", string.Empty);
            this.listView2.Groups.Add("intern", string.Empty);

            this.dataGridView1.Columns.Add("property", string.Empty);
            this.dataGridView1.Columns.Add("value", string.Empty);
            this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(EditedMetadataCallback);
            
            this.TreeView.PathSeparator = "/";
            this.TreeView.FullRowSelect = true;
            this.TreeView.ShowLines = true;
            this.TreeView.ShowPlusMinus = false;
            this.TreeView.ShowRootLines = true;
            this.TreeView.StateImageList = this.priorityIL;
            this.TreeView.Font = new System.Drawing.Font(this.listView1.Font.FontFamily, 10);
            this.TreeView.ItemHeight = 27;

            this.TreeView.Click += new EventHandler(Program.TreeView_Click);

            this.toolbutton0_0.Click += new EventHandler(GoHome);
            // other toolbuttons
            this.toolbutton0_4.Click += new EventHandler(Restart);
            this.toolbutton0_5.Click += new EventHandler(CreateResource);

            this.UpdateLanguage();
        }

        void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        internal void UpdateLanguage()
        {
            this.toolbutton0_0.Text = Localization.get_string("Item_Home");
            this.toolbutton0_1.Text = Localization.get_string("Item_Options");
            this.toolbutton0_2.Text = Localization.get_string("Item_About");
            this.toolbutton0_3.Text = Localization.get_string("Item_Help");
            this.toolbutton0_4.Text = Localization.get_string("Item_Restart");
            this.toolbutton0_5.Text = Localization.get_string("Action_New");

            this.dataGridView1.Columns[0].HeaderText = Localization.get_string("Item_Metadata");
            this.dataGridView1.Columns[1].HeaderText = Localization.get_string("Item_Value");
            
            this.listView1.Groups[0].Header = Localization.get_string("ResourceType_File_pl");
            this.listView1.Groups[1].Header = Localization.get_string("ResourceType_Code_pl");
            this.listView1.Groups[2].Header = Localization.get_string("ResourceType_Library_pl");
            this.listView1.Groups[3].Header = Localization.get_string("ResourceType_Project_pl");
            this.listView1.Groups[4].Header = Localization.get_string("ResourceType_Task_pl");

            this.listView2.Columns[0].Text = Localization.get_string("Item_Property");
            this.listView2.Columns[1].Text = Localization.get_string("Item_Value");

            this.listView2.Groups[0].Header = Localization.get_string("info_general");
            this.listView2.Groups[2].Header = Localization.get_string("info_intern");
        }

        internal void GoHome(object sender, EventArgs e)
        {
            this.panel2.Hide();
            this.panel3.Hide();
            this.panel1.Show();
        }

        internal void Restart(object sender, EventArgs e)
        {
            Application.Restart();
        }

        internal void CreateResource(object sender, EventArgs e)
        {
            new CreateResource();
        }

        internal void EditedMetadataCallback(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (string.IsNullOrWhiteSpace((string)this.dataGridView1.Rows[e.RowIndex].Cells[0].Value))
                {
                    MessageBox.Show("no empty names!");
                }
            }
            else if (e.ColumnIndex == 1)
            {

            }
        }
    }
}
