using System.Windows.Media;

namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResource
    {
        StaticInfo Info { get; }

        ImageSource Icon { get; }

        void Save();

        void Open();

        void Package();

        string Name { get; }

        string Description { get; }
    }
}
