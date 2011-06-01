using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;

namespace AHKScriptsMan.Data
{
    /// <summary>
    /// lists all resources
    /// </summary>
    public class ResourceList
    {
        static cFile[] files = new cFile[1];
        static int index1 = 0;

        static cLibrary[] libraries = new cLibrary[1];
        static int index2 = 0;

        static cProject[] projects = new cProject[1];
        static int index3 = 0;
        
        public static void Add<T>(T res)
        {
            /*switch (res.GetType().Name)
            {
                case "cFile": files[index1++] = res;
                case "cLibrary": libraries[index2++] = res;
                case "cProject": projects[index3++] = res;
                default: break;
            }
             * */
        }

        public static bool HasKey(string name)
        {
            //var result = from n in list
            //             where (n.Equals(name, StringComparison.OrdinalIgnoreCase))
            //             select n;
            /*foreach (object i in files)
            {
                
                if (i.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
             * */

            return false;
        }


    }
}
