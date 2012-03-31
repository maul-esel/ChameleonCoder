using System.Windows;

namespace ChameleonCoder
{
    /// <summary>
    /// a wrapper class to satisfy WPF
    /// </summary>
    partial class App : Application
    {
        /// <summary>
        /// executed on app startup, redirects to <see cref="ChameleonCoderApp.Open"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InitHandler(object sender, StartupEventArgs e)
        {
            ChameleonCoderApp.Open(this, e.Args);
        }
    }
}