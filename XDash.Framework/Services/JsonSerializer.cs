using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        private JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
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
