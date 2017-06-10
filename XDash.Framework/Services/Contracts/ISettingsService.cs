namespace XDash.Framework.Services.Contracts
{
    public interface ISettingsService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }
}
