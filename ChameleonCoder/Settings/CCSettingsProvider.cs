using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Xml;

namespace ChameleonCoder.Settings
{
    internal sealed class CCSettingsProvider : SettingsProvider
    {
        internal const string providerName = "CC.Xml.SettingsProvider";

        public override string ApplicationName
        {
            get
            {
                return (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            }
            set { }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(providerName, config);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection keys)
        {
            if (SettingsFile == null)
                throw new InvalidOperationException("settings file must be set first!");

            var values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty key in keys)
            {
                var value = new SettingsPropertyValue(key) { IsDirty = false };
                var node = document.SelectSingleNode("//setting[@name='" + key.Name + "']");

                if (key.SerializeAs == SettingsSerializeAs.String)
                {
                    var conv = System.ComponentModel.TypeDescriptor.GetConverter(key.PropertyType);                    
                    if (node != null)
                    {
                        var val = node.InnerText;

                        if (conv.IsValid(val))
                            value.PropertyValue = conv.ConvertFromString(val);
                        else
                            throw new ConfigurationErrorsException("could not convert value", document.SelectSingleNode("/settings/" + key.Name));
                    }
                    else
                    {
                        if (conv.IsValid(key.DefaultValue))
                            value.PropertyValue = conv.ConvertFromString(key.DefaultValue as string);
                        else
                            throw new ConfigurationErrorsException("Key not found!");
                    }
                }
                else if (key.SerializeAs == SettingsSerializeAs.Xml)
                {
                    if (node == null)
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(key.DefaultValue.ToString());
                        node = doc.DocumentElement;
                    }

                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(System.Collections.Specialized.StringCollection));
                    using (var strReader = new StringReader(node.InnerText))
                    {
                        using (var xmlReader = XmlReader.Create(strReader))
                        {
                            value.PropertyValue = value.SerializedValue = xs.Deserialize(xmlReader);
                        }
                    }
                }
                values.Add(value);
            }

            return values;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            var conv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(string));
            foreach (SettingsPropertyValue value in collection)
            {
                var node = (XmlElement)document.SelectSingleNode("//setting[@name='" + value.Name + "']");
                if (node == null)
                {
                    node = document.CreateElement("setting");
                    node.SetAttribute("name", value.Name);
                    document.SelectSingleNode("/settings").AppendChild(node);
                }

                node.InnerText = conv.ConvertToString(value.SerializedValue);
            }

            document.Save(SettingsFile);
        }

        internal string SettingsFile
        {
            get { return file; }
            set
            {
                if (file == null && File.Exists(value))
                {
                    file = value;
                    document.Load(file);
                }
            }
        }

        private string file = null;

        private XmlDocument document = new XmlDocument();
    }
}
