using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;


namespace AHKScriptsMan
{
    /// <summary>
    /// used to handle localization.
    /// This code is taken from http://www.mycsharp.de/wbb2/thread.php?threadid=61039.
    /// </summary>
    public class Localization
    {
        private static ResourceManager resMgr;

        public static void UpdateLanguage(string langID)
        {
            try
            {
                //Set Language  
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(langID);

                // Init ResourceManager  
                resMgr = new ResourceManager("AHKScriptsMan.Localizer", Assembly.GetExecutingAssembly());
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "update");
            }
          }

        public static string get_string(string pattern)
        {
            string result = string.Empty;
            try
            {
                result = resMgr.GetString(pattern);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "get");
            }
            return result;
        }
    }
}
