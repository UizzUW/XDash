using Plugin.Settings;
using XDash.Services.Contracts;

namespace XDash.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IObjectSerializerService mObjectSerializerService;

        public SettingsService(IObjectSerializerService objectSerializerService)
        {
            mObjectSerializerService = objectSerializerService;
        }

        public T Get<T>(string key)
        {
            var xml = CrossSettings.Current.GetValueOrDefault<string>(key);
            if (xml == null || string.IsNullOrEmpty(xml))
            {
                return default(T);
            }
            return mObjectSerializerService.Deserialize<T>(xml);
        }

        public void Set<T>(string key, T value)
        {
            CrossSettings.Current.AddOrUpdateValue(key, mObjectSerializerService.Serialize(value));
        }
    }
}