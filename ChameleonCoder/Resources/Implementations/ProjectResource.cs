using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Xml;
using ChameleonCoder.Resources.Base;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Implementations
{
    /// <summary>
    /// represents a project resource,
    /// inherits from ResourceBase
    /// </summary>
    public sealed class ProjectResource : ResourceBase, ICompilable
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
            get { return new Guid(this.XMLNode.Attributes["language"].Value); }
            private set { this.XMLNode.Attributes["language"].Value = value.ToString(); }
        }

        public List<Guid> compatibleLanguages { get; set; }

        #endregion

        #region ICompilable

        /// <summary>
        /// the path to which the project would be compiled
        /// </summary>
        public string compilationPath
        {
            get { return this.XMLNode.Attributes["compilation-path"].Value; }
            private set { this.XMLNode.Attributes["compilation-path"].Value = value; }
        }

        #endregion

        /// <summary>
        /// contains the project's priority (int from 0 to 2)
        /// </summary>
        internal ProjectPriority Priority
        {
            get { return (ProjectPriority)Int32.Parse(this.XMLNode.Attributes["priority"].Value); }
            private set { this.XMLNode.Attributes["priority"].Value = ((int)value).ToString(); }
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

        /// <summary>
        /// asks the user to enter a new priority and saves it
        /// </summary>
        internal void SetPriority()
        {

        }        
    }
}
