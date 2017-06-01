using XDash.Services.Contracts;

namespace XDash.iOS.Services
{
  public class IosPlatformService : IPlatformService
  {
    public Framework.Models.OperatingSystem GetOperatingSystem()
    {
      return Framework.Models.OperatingSystem.iOS;
    }
  }
}