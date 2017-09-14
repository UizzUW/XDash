using Newtonsoft.Json;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            //TypeNameHandling = TypeNameHandling.All,
            //TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
        };

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
