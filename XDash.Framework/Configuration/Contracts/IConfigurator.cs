using System.Threading.Tasks;

namespace XDash.Framework.Configuration.Contracts
{
    public interface IConfigurator
    {
        void Init(string configurationPath);
        XDashOptions GetConfiguration();
        Task SaveConfiguration(XDashOptions options);
    }
}
