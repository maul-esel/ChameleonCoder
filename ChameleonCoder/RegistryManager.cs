using Microsoft.Win32;

namespace ChameleonCoder
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal static class RegistryManager
    {
        public static readonly string fileExtension = ".ccr";

        /// <summary>
        /// registers the file extension
        /// </summary>
        internal static void RegisterExtension()
        {
            if (Registry.ClassesRoot.OpenSubKey(".ccr") == null)
            {
                RegistryKey regCCR = Registry.ClassesRoot.CreateSubKey(fileExtension, RegistryKeyPermissionCheck.ReadWriteSubTree);
                regCCR.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCR);
                regCCR.Close();

                regCCR = Registry.ClassesRoot.CreateSubKey(fileExtension + "\\Shell\\Open\\command");
                regCCR.SetValue("", "\"" + ChameleonCoderApp.AppPath + "\" \"%1\"");
                regCCR.Close();

                regCCR = Registry.ClassesRoot.CreateSubKey(fileExtension + "\\DefaultIcon");
                regCCR.SetValue("", ChameleonCoderApp.AppPath + ", 1");
                regCCR.Close();
            }
        }

        /// <summary>
        /// unregisters the file extension
        /// </summary>
        internal static void UnRegisterExtension()
        {
            if (Registry.ClassesRoot.OpenSubKey(".ccr") != null)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(fileExtension);
            }
        }
    }
}
