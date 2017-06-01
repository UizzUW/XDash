using MVPathway.Logging.Abstractions;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;
using XDash.Services.Contracts;

namespace XDash.Services
{
    class XDashBeaconService : IBeaconService
    {
        private XDashBeacon _beacon;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly ILogger _logger;

        public XDashBeaconService(IDeviceInfoService deviceInfoService, ILogger logger)
        {
            _deviceInfoService = deviceInfoService;
            _logger = logger;
        }

        public async Task StartBroadcasting()
        {
            if(_deviceInfoService.SelectedInterface == null)
            {
                _logger.LogError("No comms interface selected, broadcasting will not start.");
                return;
            }

            _beacon = new XDashBeacon
            {
                Client = _deviceInfoService.GetDeviceInfo(),
                Interface = _deviceInfoService.SelectedInterface
            };
            await _beacon.StartBroadcasting();
        }

        public void StopBroadcasting() => _beacon.StopBroadcasting();
    }
}
