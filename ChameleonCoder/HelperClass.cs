using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ChameleonCoder
{
    internal static class HelperClass
    {
    
        internal static string ToString(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.resource: return Localization.get_string("ResourceType_Resource");
                case ResourceType.file: return Localization.get_string("ResourceType_File");
                case ResourceType.code: return Localization.get_string("ResourceType_Code");
                case ResourceType.library: return Localization.get_string("ResourceType_Library");
                case ResourceType.project: return Localization.get_string("ResourceType_Project");
                case ResourceType.task: return Localization.get_string("ResourceType_Task");
                default: return string.Empty;
            }
        }

        internal static string ToString(Priority priority)
        {
            switch (priority)
            {
                case Priority.basic: return Localization.get_string("Priority_Basic");
                case Priority.middle: return Localization.get_string("Priority_Middle");
                case Priority.high: return Localization.get_string("Priority_High");
                default: return string.Empty;
            }
        }

    }
}
