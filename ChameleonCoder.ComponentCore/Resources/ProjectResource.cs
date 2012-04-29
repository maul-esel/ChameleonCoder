using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.Files;
using ChameleonCoder.Resources;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// represents a project resource,
    /// inherits from ResourceBase
    /// </summary>
    public class ProjectResource : ResourceBase
    {
        /// <summary>
        /// initializes the current instance with the given information
        /// </summary>
        /// <param name="data">the dictionary containing the resource's attributes</param>
        /// <param name="parent">the resource's parent resource,
        /// or null if the resource is a top-level resource.</param>
        public override void Update(System.Collections.Specialized.ObservableStringDictionary data, IResource parent, IDataFile file)
        {
            base.Update(data, parent, file);
            // todo: parse compatible languages
        }

        #region IResource

        /// <summary>
        /// gets the icon that represents this instance to the user
        /// </summary>
        /// <value>This is always the same as the ProjectResource's type icon</value>
        public override ImageSource Icon
        {
            get
            {
                return new BitmapImage(new Uri("pack://application:,,,/ChameleonCoder.ComponentCore;component/Images/project.png"))
                    .GetAsFrozen() as ImageSource;
            }
        }

        /// <summary>
        /// gets an icon representing a special property on this resource.
        /// </summary>
        /// <value>This icon represents the project's priority.</value>
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
        /// gets the identifier of the language module in whose coding language the source coe is written.
        /// </summary>
        /// <value>The value is taken from the "language" attribute in the resource's XML.</value>
        public Guid Language
        {
            get
            {
                Guid lang;
                string guid = Attributes["language"]; // Xml.GetAttribute("language", DataFile.NamespaceUri);

                if (!Guid.TryParse(guid, out lang))
                    lang = Guid.Empty;

                return lang;
            }
            protected set
            {
                Attributes["language"] = value.ToString("b"); // Xml.SetAttribute("language", DataFile.NamespaceUri, value.ToString());
                OnPropertyChanged("Language");
            }
        }

        /// <summary>
        /// gets a list of Guid's identifying language modules
        /// to whose coding languages this resource is compatible
        /// </summary>
        /// <value>(not yet implemented)</value>
        public Guid[] CompatibleLanguages
        {
            get { return languages.ToArray(); }
        }

        private List<Guid> languages = new List<Guid>();

        #endregion

        /// <summary>
        /// gets the project's priority
        /// </summary>
        /// <value>The value is taken from the "priority" attribute in the resource's XML.</value>
        public ProjectPriority Priority
        {
            get
            {
                ProjectPriority priority;
                string value = Attributes["priority"]; // Xml.GetAttribute("priority", DataFile.NamespaceUri);
                if (Enum.TryParse<ProjectPriority>(value, out priority))
                	return priority;
                return ProjectPriority.Low;
            }
            protected set
            {
                Attributes["priority"] = value.ToString(); // Xml.SetAttribute("priority", DataFile.NamespaceUri, value.ToString());
                OnPropertyChanged("Priority");
            }
        }

        /// <summary>
        /// gets the display name of this resource's language module
        /// </summary>
        [ResourceProperty(CommonResourceProperty.Language, ResourcePropertyGroup.ThisClass)]
        public string LanguageName
        {
            get
            {
                Plugins.ILanguageModule module;
                if (File.App.PluginMan.TryGetModule(Language, out module))
                    return module.Name;
                return "error: module could not be found";
            }
        }

        /// <summary>
        /// gets a string contatining the dislay names of the language modules to which the source code is compatible.
        /// </summary>
        [ResourceProperty(CommonResourceProperty.CompatibleLanguages, ResourcePropertyGroup.ThisClass, IsReadOnly = true)]
        public string CompatibleLanguagesNames
        {
            get
            {
                string list = string.Empty;
                foreach (Guid lang in CompatibleLanguages)
                {
                    Plugins.ILanguageModule module;
                    if (File.App.PluginMan.TryGetModule(lang, out module))
                        list += module.Name + "; ";

                }
                return list;
            }
        }

        /// <summary>
        /// gets the localized name of the project's priority level.
        /// </summary>
        /// <value>The value is taken from the localized resource file.</value>
        [ResourceProperty("NameOfPriority", ResourcePropertyGroup.ThisClass, IsReferenceName = true)]
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

        /// <summary>
        /// gets the localized name of the <see cref="Priority"/> property.
        /// </summary>
        /// <value>The value is taken from the localized resource file.</value>
        public static string NameOfPriority
        {
            get { return Properties.Resources.Info_Priority; }
        }

        internal const string Key = "{ca90cff4-dae6-4542-b516-771d01b39de9}";
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
