using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChameleonCoder.ComponentCore.RichContentMembers;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.ComponentCore
{
    [CCPlugin]
    public sealed class RichContentFactory : IRichContentFactory
    {
        #region IPlugin

        /// <summary>
        /// gets the 'about'-information for this plugin
        /// </summary>
        public string About { get { return "© 2011 maul.esel - CC integrated RichContent types"; } }

        /// <summary>
        /// gets the author(s) of this plugin
        /// </summary>
        public string Author { get { return "maul.esel"; } }

        /// <summary>
        /// gets a short description of this plugin
        /// </summary>
        public string Description { get { return "provides the ChameleonCoder integrated RichContent types"; } }

        /// <summary>
        /// gets an icon representing this plugin to the user
        /// </summary>
        public ImageSource Icon { get { return new BitmapImage(new Uri("pack://application:,,,/Images/logo.png")); } }

        /// <summary>
        /// gets an globally unique identifier identifying the plugin
        /// </summary>
        public Guid Identifier { get { return new Guid("{3030d050-d7e5-4994-939f-39647a91ad65}"); } }

        /// <summary>
        /// gets the plugin's name
        /// </summary>
        public string Name { get { return "ChameleonCoder.ComponentCore RichContent types"; } }

        /// <summary>
        /// gets the plugin's current version
        /// </summary>
        public string Version { get { return "0.0.0.1"; } }

        /// <summary>
        /// initializes the plugin
        /// </summary>
        public void Initialize()
        {
            ContentMemberManager.RegisterContentMember(typeof(FieldMember), Guid.Parse(FieldMember.Key), this);
            ContentMemberManager.RegisterContentMember(typeof(MethodMember), Guid.Parse(MethodMember.Key), this);
            ContentMemberManager.RegisterContentMember(typeof(VariableMember), Guid.Parse(VariableMember.Key), this);
            ContentMemberManager.RegisterContentMember(typeof(FunctionMember), Guid.Parse(FunctionMember.Key), this);
            ContentMemberManager.RegisterContentMember(typeof(ParameterMember), Guid.Parse(ParameterMember.Key), this);
            ContentMemberManager.RegisterContentMember(typeof(ReturnValueMember), Guid.Parse(ReturnValueMember.Key), this);
        }

        /// <summary>
        /// prepares the plugin for closing the application
        /// </summary>
        public void Shutdown() { }

        #endregion

        /// <summary>
        /// creates an instance of the given type
        /// </summary>
        /// <param name="memberType">the type to create an instance of</param>
        /// <param name="data">the XmlElement representing the member</param>
        /// <param name="parent">the parent member</param>
        /// <returns>the newly created instance</returns>
        public IContentMember CreateInstance(Type memberType, System.Xml.XmlElement data, IContentMember parent)
        {
            IContentMember member = Activator.CreateInstance(memberType, new object[2] { data, parent }) as IContentMember;

            return member;
        }

        /// <summary>
        /// creates a new ContentMember of the given Type, using the given name and parent member
        /// </summary>
        /// <param name="type">the type of the member to create</param>
        /// <param name="name">the name of the new member</param>
        /// <param name="parent">the parent member or null</param>
        /// <returns>a dictionary containing the attributes the new instance should have</returns>
        public IDictionary<string, string> CreateMember(Type type, string name, IContentMember parent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// gets a list of all types registered by this factory
        /// </summary>
        /// <returns>the Type-Array</returns>
        public IEnumerable<Type> RegisteredTypes { get { return registeredTypesArray; } }

        private static Type[] registeredTypesArray = new Type[0];
    }
}
