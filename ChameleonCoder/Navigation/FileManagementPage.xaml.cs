using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Shared logic for FileManagementPage.xaml
    /// </summary>
    internal sealed partial class FileManagementPage : CCPageBase
    {
        internal FileManagementPage(DataFile file)
        {
            Initialize(new ViewModel.FileManagementPageModel(file));
            InitializeComponent();
        }
    }
}
