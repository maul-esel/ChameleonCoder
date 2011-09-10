namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to implement by resources that can be compiled
    /// </summary>
    public interface ICompilable : ILanguageResource
    {
        /// <summary>
        /// the path to which the resource would be compiled
        /// </summary>
        string CompilationPath { get; }
    }
}
