namespace XDash.Framework.Services.Contracts
{
    public interface IObjectSerializer<TOutput>
        where TOutput : class
    {
        TOutput Serialize<TInput>(TInput obj);
        TInput Deserialize<TInput>(TOutput data);
    }
}
