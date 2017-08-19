using Plugin.Settings;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly IJsonSerializer _jsonSerializer;

        public SettingsRepository(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public T Get<T>(string key)
        {
            var json = CrossSettings.Current.GetValueOrDefault(key, string.Empty);
            return string.IsNullOrEmpty(json) ? default(T) : _jsonSerializer.Deserialize<T>(json);
        }

        public void Set<T>(string key, T value)
        {
            CrossSettings.Current.AddOrUpdateValue(key, _jsonSerializer.Serialize(value));
        }
    }
}