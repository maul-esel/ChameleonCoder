using System;
using System.Collections.Generic;
using ChameleonCoder.Plugins;
using Res = ChameleonCoder.Properties.Resources;

namespace ChameleonCoder.ViewModel
{
    [DefaultRepresentation(typeof(NewResourceDialog))]
    internal sealed class NewResourceDialogModel : ViewModelBase
    {
        internal NewResourceDialogModel(ChameleonCoderApp app)
            : base(app)
        {
            templateList = new List<ITemplate>(App.PluginMan.GetTemplates());

            foreach (var type in App.ResourceTypeMan.GetResourceTypes())
            {
                if (!Attribute.IsDefined(type, typeof(NoWrapperTemplateAttribute)))
                {
                    templateList.Add(new AutoTemplate(type));
                }
            }

            foreach (var template in templateList)
            {
                template.Initialize(App);

                string group = string.IsNullOrWhiteSpace(template.Group) ? Res.PropertyGroup_General : template.Group;
                if (!groupList.Contains(group))
                {
                    groupList.Add(group);
                }
            }
        }

        public IList<ITemplate> Templates
        {
            get { return templateList; }
        }

        private readonly List<ITemplate> templateList = null;

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
