using Microsoft.Win32;

namespace ChameleonCoder
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal static class RegistryManager
    {
        public const string FileExtension = ".ccr";

        /// <summary>
        /// registers the file extension
        /// </summary>
        internal static void RegisterExtension()
        {
            if (Registry.ClassesRoot.OpenSubKey(FileExtension) == null)
            {
                RegistryKey regCCR = Registry.ClassesRoot.CreateSubKey(FileExtension, RegistryKeyPermissionCheck.ReadWriteSubTree);
                regCCR.SetValue("", ChameleonCoder.Properties.Resources.Ext_CCR);
                regCCR.Close();

                regCCR = Registry.ClassesRoot.CreateSubKey(FileExtension + "\\Shell\\Open\\command");
                regCCR.SetValue("", "\"" + ChameleonCoderApp.AppPath + "\" \"%1\"");
                regCCR.Close();

                regCCR = Registry.ClassesRoot.CreateSubKey(FileExtension + "\\DefaultIcon");
                regCCR.SetValue("", ChameleonCoderApp.AppPath + ", 1");
                regCCR.Close();
            }
        }

        /// <summary>
        /// unregisters the file extension
        /// </summary>
        internal static void UnRegisterExtension()
        {
            if (Registry.ClassesRoot.OpenSubKey(FileExtension) != null)
            {
                Registry.ClassesRoot.DeleteSubKeyTree(FileExtension);
            }
        }
    }
}
