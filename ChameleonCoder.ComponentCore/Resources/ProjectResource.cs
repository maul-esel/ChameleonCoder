using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a project resource,
    /// inherits from ResourceBase
    /// </summary>
    public class ProjectResource : ResourceBase, ICompilable
    {
        /// <summary>
        /// instantiates a new instance of the ProjectResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the xpath to the resource's main element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public override void Init(XmlElement node, IResource parent)
        {
            base.Init(node, parent);
            compatibleLanguages = new List<Guid>();
        }

        #region IResource

        public override ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/project.png")).GetAsFrozen() as ImageSource; } }

        public override ImageSource SpecialVisualProperty
        {
            get
            {
                string id;
                switch (Priority)
                {
                    default:
                    case ProjectPriority.basic: id = "low"; break;
                    case ProjectPriority.middle: id = "middle"; break;
                    case ProjectPriority.high: id = "high"; break;
                }
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/Priority/" + id + ".png")).GetAsFrozen() as ImageSource;
            }
        }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// the GUID of the language in which the project is written
        /// </summary>
        public Guid language
        {
            get
            {
                Guid lang;
                string guid = Xml.GetAttribute("language");

                if (!Guid.TryParse(guid, out lang))
                    lang = Guid.Empty;

                return lang;
            }
            protected set
            {
                Xml.SetAttribute("language", value.ToString());
                OnPropertyChanged("language");
            }
        }

        public List<Guid> compatibleLanguages { get; set; }

        #endregion

        #region ICompilable

        /// <summary>
        /// the path to which the project would be compiled
        /// </summary>
        [ResourceProperty(CommonResourceProperty.CompilationPath, ResourcePropertyGroup.ThisClass)]
        public string compilationPath
        {
            get
            {
                return Xml.GetAttribute("compilation-path");
            }
            protected set
            {
                Xml.SetAttribute("compilation-path", value);
                OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        public ProjectPriority Priority
        {
            get
            {
                ProjectPriority priority;
                string value = Xml.GetAttribute("priority");
                Enum.TryParse<ProjectPriority>(value, out priority);
                return priority;
            }
            protected set
            {
                Xml.SetAttribute("priority", value.ToString());
                OnPropertyChanged("Priority");
            }
        }

        /// <summary>
        /// defines a project's priority
        /// </summary>
        public enum ProjectPriority
        {
            /// <summary>
            /// the project has the default (low) priority
            /// </summary>
            basic,

            /// <summary>
            /// the project has a slightly higher priority (middle)
            /// </summary>
            middle,

            /// <summary>
            /// the project has a high priority
            /// </summary>
            high
        }

        [ResourceProperty(CommonResourceProperty.Language, ResourcePropertyGroup.ThisClass)]
        public string LanguageName
        {
            get
            {
                Plugins.ILanguageModule module;
                if (Plugins.PluginManager.TryGetModule(language, out module))
                    return module.Name;
                return "error: module could not be found";
            }
        }

        [ResourceProperty(CommonResourceProperty.CompatibleLanguages, ResourcePropertyGroup.ThisClass, IsReadOnly = true)]
        public string CompatibleLanguagesNames
        {
            get
            {
                string list = string.Empty;
                foreach (Guid lang in compatibleLanguages)
                {
                    Plugins.ILanguageModule module;
                    if (Plugins.PluginManager.TryGetModule(lang, out module))
                        list += module.Name + "; ";

                }
                return list;
            }
        }

        [ResourceProperty("nameof_PriorityName", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
        public string PriorityName
        {
            get
            {
                switch (Priority)
                {
                    default:
                    case ProjectPriority.basic: return Properties.Resources.Priority_Low;
                    case ProjectPriority.middle: return Properties.Resources.Priority_Middle;
                    case ProjectPriority.high: return Properties.Resources.Priority_High;
                }
            }
        }

        public string nameof_PriorityName
        {
            get { return Properties.Resources.Info_Priority; }
        }

        internal const string Alias = "project";
    }
}
