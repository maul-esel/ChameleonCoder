using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a project resource,
    /// inherits from ResourceBase
    /// </summary>
    public class ProjectResource : ResourceBase, ICompilable, IAllowChildren
    {
        /// <summary>
        /// instantiates a new instance of the ProjectResource class
        /// </summary>
        /// <param name="xml">the XmlDocument that contains the resource's definition</param>
        /// <param name="xpath">the xpath to the resource's main element</param>
        /// <param name="datafile">the file that contains the definition</param>
        public ProjectResource(XmlNode node)
            : base(node)
        {
            this.compatibleLanguages = new List<Guid>();
            this.children = new ResourceCollection();
        }

        #region IResource

        public override ImageSource Icon { get { return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/ResourceType/project.png")); } }

        public override ImageSource SpecialVisualProperty
        {
            get
            {
                switch (this.Priority)
                {
                    case ProjectPriority.basic: return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/Priority/low.png"));
                    case ProjectPriority.middle: return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/Priority/middle.png"));
                    case ProjectPriority.high: return new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Images/Priority/high.png"));
                }
                return base.SpecialVisualProperty;
            }
        }

        #endregion

        #region ILanguageResource

        /// <summary>
        /// the GUID of the language in which the project is written
        /// </summary>
        public Guid language
        {
            get { return new Guid(this.Xml.Attributes["language"].Value); }
            protected set
            {
                this.Xml.Attributes["language"].Value = value.ToString();
                this.OnPropertyChanged("language");
            }
        }

        public List<Guid> compatibleLanguages { get; set; }

        #endregion

        #region ICompilable

        /// <summary>
        /// the path to which the project would be compiled
        /// </summary>
        public string compilationPath
        {
            get
            {
                string result = null;

                try { result = this.Xml.Attributes["compilation-path"].Value; }
                catch (NullReferenceException) { }

                return result;
            }
            protected set
            {
                this.Xml.Attributes["compilation-path"].Value = value;
                this.OnPropertyChanged("compilationPath");
            }
        }

        #endregion

        #region IEnumerable

        public override IEnumerator<PropertyDescription> GetEnumerator()
        {
            IEnumerator<PropertyDescription> baseEnum = base.GetEnumerator();
            while (baseEnum.MoveNext())
                yield return baseEnum.Current;

            string langName = string.Empty;
            try { langName = LanguageModules.LanguageModuleHost.GetModule(this.language).LanguageName; }
            catch(NullReferenceException) { }

            yield return new PropertyDescription("language", langName, "project");

            string list = string.Empty;
            foreach (Guid lang in this.compatibleLanguages)
            {
                try { list += LanguageModules.LanguageModuleHost.GetModule(lang).LanguageName + "; "; }
                catch (NullReferenceException) { }
            }

            yield return new PropertyDescription("compatible languages", list, "project");

            yield return new PropertyDescription("compilation path", this.compilationPath, "project");
        }

        #endregion

        #region IAllowChildren

        public ResourceCollection children { get; protected set; }

        #endregion

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        public ProjectPriority Priority
        {
            get { return (ProjectPriority)Int32.Parse(this.Xml.Attributes["priority"].Value); }
            protected set
            {
                this.Xml.Attributes["priority"].Value = ((int)value).ToString();
                this.OnPropertyChanged("Priority");
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
    }
}
