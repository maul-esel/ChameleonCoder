using System.Windows.Controls;

namespace ChameleonCoder
{
    public class TabContext
    {
        public TabContext(string title, Page content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; set; }
        public Page Content { get; set; }
    }
}
