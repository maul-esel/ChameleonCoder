using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace ChameleonCoder
{
    [ComVisible(true)]
    [ProgId("ChameleonCoder.Application"), Guid("{712fc748-468f-45db-ab09-e472b6a97b69}")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual), ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class ChameleonCoderApp
    {
        /// <summary>
        /// registers the file extensions *.ccr
        /// </summary>
        internal void RegisterExtension()
        {
            RegistryManager.RegisterExtension();
        }

        /// <summary>
        /// unregisters the file extensions *.ccr
        /// </summary>
        internal void UnRegisterExtension()
        {
            RegistryManager.UnRegisterExtension();
        }
    }
}
