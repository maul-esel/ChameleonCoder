using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AHKScriptsMan.Window
{
    class Scintilla
    {
        #region dllimport
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateWindowEx(uint hExStyle, string className, string title, uint hStyle, int x, int y, int w, int h, IntPtr parent);
        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);
        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        #endregion

        #region properties
        private static IntPtr hModule = LoadLibrary(Application.StartupPath + "\\#Extern\\SciLexer.dll");

        private static IntPtr fn = GetProcAddress(hModule, "Scintilla_DirectFunction");

        private IntPtr handle;


        private IntPtr directPtr;
        #endregion

        public Scintilla(IntPtr parent, int x, int y, int w, int h)
        {
            handle = CreateWindowEx(0x200, "Scintilla", "", 0x40000000 | 0x10000000, x, y, w, h, parent);
            directPtr = SendMessage(handle, 2185, IntPtr.Zero, IntPtr.Zero);
        }

        public bool Destroy()
        {
            return DestroyWindow(handle);
        }

        public IntPtr ADDREFDOCUMENT(IntPtr pDoc)
        {
            //return fn(directPtr, 2376, 0, pDoc);
            return handle;
        }
    }
}
