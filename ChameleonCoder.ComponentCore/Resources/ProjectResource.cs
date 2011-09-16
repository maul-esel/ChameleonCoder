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
        /// initializes the instance
        /// </summary>
        /// <param name="xml">the XmlElement that contains the resource's definition</param>
        /// <param name="parent">the resource's parent resource</param>
        public override void Initialize(XmlElement data, IResource parent)
        {
            base.Initialize(data, parent);
            // todo: parse compatible languages
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
                    case ProjectPriority.Low: id = "low"; break;
                    case ProjectPriority.Middle: id = "middle"; break;
                    case ProjectPriority.High: id = "high"; break;
                }
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/Priority/" + id + ".png")).GetAsFrozen() as ImageSource;
            }
        }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// the GUID of the Language in which the project is written
        /// </summary>
        public Guid Language
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
                OnPropertyChanged("Language");
            }
        }

        public IEnumerable<Guid> CompatibleLanguages
        {
            get { return languages; }
        }

        private List<Guid> languages = new List<Guid>();

        #endregion

        #region ICompilable

        /// <summary>
        /// the path to which the project would be compiled
        /// </summary>
        [ResourceProperty(CommonResourceProperty.CompilationPath, ResourcePropertyGroup.ThisClass)]
        public string CompilationPath
        {
            get
            {
                return Xml.GetAttribute("compilation-path");
            }
            protected set
            {
                Xml.SetAttribute("compilation-path", value);
                OnPropertyChanged("CompilationPath");
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
                if (Enum.TryParse<ProjectPriority>(value, out priority))
                	return priority;
                return ProjectPriority.Low;
            }
            protected set
            {
                Xml.SetAttribute("priority", value.ToString());
                OnPropertyChanged("Priority");
            }
        }


        [ResourceProperty(CommonResourceProperty.Language, ResourcePropertyGroup.ThisClass)]
        public string LanguageName
        {
            get
            {
                Plugins.ILanguageModule module;
                if (Plugins.PluginManager.TryGetModule(Language, out module))
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
                foreach (Guid lang in CompatibleLanguages)
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
                    case ProjectPriority.Low: return Properties.Resources.Priority_Low;
                    case ProjectPriority.Middle: return Properties.Resources.Priority_Middle;
                    case ProjectPriority.High: return Properties.Resources.Priority_High;
                }
            }
        }

        public static string nameof_PriorityName
        {
            get { return Properties.Resources.Info_Priority; }
        }

        internal const string Alias = "project";
    }

    /// <summary>
    /// defines a project's priority
    /// </summary>
    public enum ProjectPriority
    {
        /// <summary>
        /// the project has the default (low) priority
        /// </summary>
        Low,

        /// <summary>
        /// the project has a slightly higher priority (middle)
        /// </summary>
        Middle,

        /// <summary>
        /// the project has a high priority
        /// </summary>
        High
    }
}
