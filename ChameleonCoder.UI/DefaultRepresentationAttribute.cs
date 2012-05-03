namespace ChameleonCoder.UI
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = false), System.Runtime.InteropServices.ComVisible(false)]
    internal sealed class DefaultRepresentationAttribute : System.Attribute
    {
        internal DefaultRepresentationAttribute(System.Type representationType)
        {
            type = representationType;
        }

        internal System.Type RepresentationType
        {
            get { return type; }
        }

        private readonly System.Type type;
    }
}
