using MVPathway.Logging.Abstractions;
using MVPathway.MVVM.Abstractions;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    class XDashBeaconService : IBeaconService
    {
        private readonly IDiContainer _container;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly ILogger _logger;
        private IXDashBeacon _beacon;

        public XDashBeaconService(IDiContainer container,
                                  IDeviceInfoService deviceInfoService,
                                  ILogger logger)
        {
            _container = container;
            _deviceInfoService = deviceInfoService;
            _logger = logger;
        }

        public async Task StartBroadcasting()
        {
            if (_deviceInfoService.SelectedInterface == null)
            {
                _logger.LogError("No comms interface selected, broadcasting will not start.");
                return;
            }

            _beacon = _container.Resolve<IXDashBeacon>();
            _beacon.Client = _deviceInfoService.GetDeviceInfo();
            _beacon.Interface = _deviceInfoService.SelectedInterface;
            await _beacon.StartBroadcasting();
        }

        public void StopBroadcasting() => _beacon.StopBroadcasting();
    }
}
