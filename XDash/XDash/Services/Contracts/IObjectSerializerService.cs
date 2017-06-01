namespace XDash.Services.Contracts
{
    public interface IObjectSerializerService
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string data);
    }
}
