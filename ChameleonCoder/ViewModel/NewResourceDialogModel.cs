using System;
using System.Collections.Generic;
using ChameleonCoder.Plugins;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(NewResourceDialog))]
    internal sealed class NewResourceDialogModel : ViewModelBase
    {
        internal NewResourceDialogModel()
        {
            foreach (var type in ChameleonCoderApp.RunningObject.ResourceTypeMan.GetResourceTypes())
                if (!Attribute.IsDefined(type, typeof(NoWrapperTemplateAttribute)))
                    templateList.Add(new AutoTemplate(type));

            foreach (var template in templateList)
            {
                string group = string.IsNullOrWhiteSpace(template.Group) ? Properties.Resources.PropertyGroup_General : template.Group;
                if (!groupList.Contains(group))
                    groupList.Add(group);
            }
        }

        public IList<ITemplate> Templates
        {
            get { return templateList; }
        }

        private readonly List<ITemplate> templateList = new List<ITemplate>(ChameleonCoderApp.RunningObject.PluginMan.GetTemplates());

        public IList<string> Groups
        {
            get { return groupList; }
        }

        private readonly List<string> groupList = new List<string>();

        #region localization

        public static string Info_Author { get { return Res.Info_Author; } }

        public static string Info_Version { get { return Res.Info_Version; } }

        public static string Action_Cancel { get { return Res.Action_Cancel; } }

        public static string Action_OK { get { return Res.Action_OK; } }

        #endregion
    }
}
