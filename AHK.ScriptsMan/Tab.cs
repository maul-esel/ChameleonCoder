using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AHKScriptsMan.Window
{
    /// <summary>
    /// struct used to manage tab information
    /// </summary>
    public struct TCITEM
    {
        /// <summary>
        /// defines which information is set / queried
        /// </summary>
        public uint mask;
        public uint dwState;
        public uint dwStateMask;
        public string pszText;
        public int cchTextMax;
        public int iImage;
        public uint lparam;
    }

    public class Tab
    {
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, TCITEM lParam);
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, out TCITEM lParam);
   
        public static bool SetLParam(IntPtr tab, int index, uint lparam)
        {
            TCITEM item = new TCITEM();
            item.mask = 0x008;
            item.lparam = lparam;

            return SendMessage(tab, 0x1361, index, item);
        }

        public static uint GetLParam(IntPtr tab, int index)
        {
            TCITEM item = new TCITEM();
            item.mask = 0x008;
            SendMessage(tab, 0x1360, index, out item);
            return item.lparam;
        }
    }
}
