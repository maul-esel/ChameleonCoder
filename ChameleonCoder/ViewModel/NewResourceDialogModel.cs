using System;
using System.Collections.Generic;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Management;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    internal sealed class NewResourceDialogModel : ViewModelBase
    {
        internal NewResourceDialogModel()
        {
            foreach (var type in ResourceTypeManager.GetResourceTypes())
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

        private readonly List<ITemplate> templateList = new List<ITemplate>(PluginManager.GetTemplates());

        public IList<string> Groups
        {
            get { return groupList; }
        }

        private readonly List<string> groupList = new List<string>();

        #region localization

        public string Info_Author { get { return Res.Info_Author; } }

        public string Info_Version { get { return Res.Info_Version; } }

        public string Action_Cancel { get { return Res.Action_Cancel; } }

        public string Action_OK { get { return Res.Action_OK; } }

        #endregion
    }
}
