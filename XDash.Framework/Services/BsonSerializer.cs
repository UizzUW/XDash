using System.Text;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class BsonSerializer : IBsonSerializer
    {
        private readonly IJsonSerializer _jsonSerializer;

        public BsonSerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public byte[] Serialize<T>(T data)
        {
            var jsonObject = _jsonSerializer.Serialize(data);
            return Encoding.UTF8.GetBytes(jsonObject);
        }

        public T Deserialize<T>(byte[] data)
        {
            var jsonData = Encoding.UTF8.GetString(data, 0, data.Length);
            return _jsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
