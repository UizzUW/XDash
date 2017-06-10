using System.Text;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class BinarySerializer : IBinarySerializer
    {
        private IJsonSerializer _jsonSerializer;

        public BinarySerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public byte[] Serialize<T>(T data)
        {
            var jsonObject = _jsonSerializer.Serialize(data);
            return Encoding.Unicode.GetBytes(jsonObject);
        }

        public T Deserialize<T>(byte[] data)
        {
            var jsonData = Encoding.Unicode.GetString(data, 0, data.Length);
            return _jsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
