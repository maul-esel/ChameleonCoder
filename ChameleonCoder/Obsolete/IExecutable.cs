namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface that may be used to execute resources. However, this may also be dropped
    /// </summary>
    [System.Obsolete("replaced by ILanguageModule::Execute()")]
    public interface IExecutable : ILanguageResource
    {
    }
}
