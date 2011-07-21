using System;

namespace ChameleonCoder.Resources
{
    public sealed class PropertyDescription
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        public string Group { get; private set; }

        public bool IsReadOnly { get; set; }

        public PropertyDescription(string name, object value, string group)
        {
            this.Name = name;
            if (value == null)
                this.Value = string.Empty;
            else
                this.Value = value.ToString();
            this.Group = group;
        }

        public PropertyDescription(string name, object value, Type group)
            : this(name, value, Resources.Management.ResourceTypeManager.GetInfo(group).DisplayName)
        { }

        public PropertyDescription(string name, object value, Resources.Interfaces.IResource group)
            : this(name, value, group.GetType())
        { }
    }
}
