using Plugin.Settings;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IJsonSerializer _jsonSerializer;

        public SettingsService(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public T Get<T>(string key)
        {
            var xml = CrossSettings.Current.GetValueOrDefault<string>(key);
            if (xml == null || string.IsNullOrEmpty(xml))
            {
                return default(T);
            }
            return _jsonSerializer.Deserialize<T>(xml);
        }

        public void Set<T>(string key, T value)
        {
            CrossSettings.Current.AddOrUpdateValue(key, _jsonSerializer.Serialize(value));
        }
    }
}