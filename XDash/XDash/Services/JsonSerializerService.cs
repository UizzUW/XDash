using Newtonsoft.Json;
using XDash.Services.Contracts;

namespace XDash.Services
{
  public class JsonSerializerService : IObjectSerializerService
  {
    public string Serialize<T>(T obj)
    {
      return JsonConvert.SerializeObject(obj);
    }

    public T Deserialize<T>(string xml)
    {
      return JsonConvert.DeserializeObject<T>(xml);
    }
  }
}
