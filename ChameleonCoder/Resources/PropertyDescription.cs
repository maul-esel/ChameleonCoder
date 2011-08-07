using System.Reflection;

namespace ChameleonCoder.Resources
{
    internal sealed class PropertyDescription
    {
        internal PropertyDescription(string name, string group, PropertyInfo info, object instance, bool isReadOnly)
        {
            Name = name;
            Group = group;
            _info = info;
            _instance = instance;
            _readonly = isReadOnly;
        }

        object _instance;
        PropertyInfo _info;
        bool _readonly;

        public string Name { get; private set; }

        public string Group { get; private set; }

        public bool IsReadOnly { get { return _readonly; } }

        public object Value
        {
            get
            {
                var method = _info.GetGetMethod();
                if (method != null)
                    return method.Invoke(_instance, null);
                return null;
            }
            set
            {
                if (_readonly)
                    return;
                var method = _info.GetSetMethod();
                if (method != null)
                    method.Invoke(_instance, new object[1] { value });
            }
        }
    }
}
