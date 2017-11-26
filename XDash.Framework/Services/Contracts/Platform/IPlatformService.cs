using XDash.Framework.Models;

namespace XDash.Framework.Services.Contracts.Platform
{
    public interface IPlatformService
    {
        OperatingSystem OS { get; }

        string ConfigurationPath { get; }

        void ExitApp();
    }
}
