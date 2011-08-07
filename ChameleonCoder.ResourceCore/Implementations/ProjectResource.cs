using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.ResourceCore
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
            this.compatibleLanguages = new List<Guid>();
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/project.png")).GetAsFrozen() as ImageSource; } }

        object lock_special = new object();
        public override ImageSource SpecialVisualProperty
        {
            get
            {
                lock (lock_special)
                {
                    switch (this.Priority)
                    {
                        case ProjectPriority.basic: return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/Priority/low.png"));
                        case ProjectPriority.middle: return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/Priority/middle.png"));
                        case ProjectPriority.high: return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ResourceCore;component/Images/Priority/high.png"));
                    }
                    return base.SpecialVisualProperty;
                }
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
                return new Guid(Xml.Attributes["language"].Value);
            }
            protected set
            {
                Xml.Attributes["language"].Value = value.ToString();
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
                string result = null;

                try { result = Xml.Attributes["compilation-path"].Value; }
                catch (NullReferenceException) { }

                return result;
            }
            protected set
            {
                Xml.Attributes["compilation-path"].Value = value;
                OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        public ProjectPriority Priority
        {
            get { return (ProjectPriority)Int32.Parse(this.Xml.Attributes["priority"].Value); }
            protected set
            {
                Xml.Attributes["priority"].Value = ((int)value).ToString();
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
    }
}
