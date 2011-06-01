using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace AHKScriptsMan.Window
{
    class DataProvider
    {
        public static ImageList GetImageList(int min, int count)
        {
            ImageList il = new ImageList();
            for (int i = min; i <= count + min; i++)
            {
                try
                {
                    il.Images.Add("icon" + i, (Image)AppResources.ResourceManager.GetObject("icon" + i));
                }
                catch
                {
                }
            }
            return il;
        }

    }
}
