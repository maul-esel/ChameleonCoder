using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void OpenResourceView(object sender, EventArgs e)
        {
            bool found = false;
            int i = -1;
            foreach (object o in App.Gui.Tabs.Items)
            {
                i++;
                if (((KeyValuePair<string, Page>)o).Value.GetType() == typeof(ResourceListPage))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                i = App.Gui.Tabs.Items.Add(new KeyValuePair<string, Page>("resources", new Navigation.ResourceListPage()));
            }
            App.Gui.Tabs.SelectedIndex = i;
        }

        private void CreateResource(object sender, EventArgs e)
        {

        }

        private void OpenConfiguration(object sender, EventArgs e)
        {

        }
    }
}
