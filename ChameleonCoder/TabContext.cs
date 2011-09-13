using System.Windows.Controls;

namespace ChameleonCoder
{
    internal sealed class TabContext
    {
        internal TabContext(string title, Page content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; set; }
        public Page Content { get; set; }
    }
}
