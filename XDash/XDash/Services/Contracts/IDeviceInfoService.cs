using Sockets.Plugin.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDash.Framework.Models;

namespace XDash.Services.Contracts
{
  public interface IDeviceInfoService
  {
    IEnumerable<ICommsInterface> Interfaces { get; }
    ICommsInterface SelectedInterface { get; set; }
    Task Init();
    XDashClient GetDeviceInfo();
    void RenameDevice(string name);
  }
}
