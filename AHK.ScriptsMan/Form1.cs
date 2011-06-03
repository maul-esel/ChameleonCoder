using System;
using System.Windows.Forms;

namespace AHKScriptsMan
{
    public partial class MainWin : Form
    {
        public ListViewGroup[] groups { get; set; }

        public MainWin()
        {
            InitializeComponent();
            
            this.listView1.Columns.Add(string.Empty);
            this.listView1.Columns.Add(string.Empty);
            this.listView1.View = View.Details;

            this.listView1.ShowGroups = true;
            this.groups = new ListViewGroup[5];
            this.groups[0] = this.listView1.Groups.Add("files", string.Empty);
            this.groups[1] = this.listView1.Groups.Add("codes", string.Empty);
            this.groups[2] = this.listView1.Groups.Add("libraries", string.Empty);
            this.groups[3] = this.listView1.Groups.Add("projects", string.Empty);
            this.groups[4] = this.listView1.Groups.Add("tasks", string.Empty);

            this.TreeView.ImageList = Window.DataProvider.GetImageList(9, 5);
            this.TreeView.PathSeparator = "\\";

            this.DragDrop += new DragEventHandler(Program.Window_DragDrop); // geeignet für Dateien?
            this.TreeView.Click += new EventHandler(Program.TreeView_Click);

            this.UpdateLanguage();
        }

        public void UpdateLanguage()
        {
            this.toolbutton0_1.Text = Localization.get_string("Item_Options");
            this.toolbutton0_2.Text = Localization.get_string("Item_About");
            this.groups[0].Header = Localization.get_string("ResourceType_file_pl");
        }

    }
}
