using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace XDash.Framework.Services
{
    public static class DataSerializerService
    {
        private static JsonSerializerSettings sJsonSerializingSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
        };

        public static byte[] Serialize(Dictionary<string, object> data)
        {
            var jsonObject = JsonConvert.SerializeObject(data, sJsonSerializingSettings);
            return Encoding.Unicode.GetBytes(jsonObject);
        }

        public static Dictionary<string,object> Deserialize(byte[] data)
        {
            var jsonData = Encoding.Unicode.GetString(data, 0, data.Length);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData, sJsonSerializingSettings);
        }
    }
}
