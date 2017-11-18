using MVPathway.Logging.Abstractions;
using MVPathway.MVVM.Abstractions;
using MVPathway.Navigation.Abstractions;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    class XDashBeaconService : IBeaconService
    {
        private readonly IDiContainer _container;
        private readonly INavigator _navigator;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly ILogger _logger;
        private IXDashBeacon _beacon;

        public XDashBeaconService(IDiContainer container,
                                  INavigator navigator,
                                  IDeviceInfoService deviceInfoService,
                                  ILogger logger)
        {
            _container = container;
            _navigator = navigator;
            _deviceInfoService = deviceInfoService;
            _logger = logger;
        }

        public async Task StartBroadcasting()
        {
            if (await _deviceInfoService.GetSelectedInterface() == null)
            {
                await _navigator.DisplayAlertAsync("Error", "Please select network card first.", "Ok");
                return;
            }

            _beacon = _container.Resolve<IXDashBeacon>();
            await _beacon.StartListening();
        }

        public async Task StopBroadcasting()
        {
            if (_beacon == null)
            {
                return;
            }
            await _beacon.StopListening();
            _beacon = null;
        }
    }
}
