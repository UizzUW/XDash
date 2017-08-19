namespace XDash.Framework.Services.Contracts
{
    public interface ISettingsRepository : MVPathway.Settings.Abstractions.ISettingsRepository
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }
}
