using MVPathway.MVVM.Abstractions;
using MVPathway.Navigation.Abstractions;
using System;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    class XDashRadarService : IRadarService
    {
        private readonly IDiContainer _container;
        private readonly INavigator _navigator;
        private readonly IDeviceInfoService _deviceInfoService;
        private IXDashRadar _radar;

        private Action<DasherFoundEventArgs> _onDasherFound;

        public XDashRadarService(IDiContainer container,
                                 INavigator navigator,
                                 IDeviceInfoService deviceInfoService)
        {
            _container = container;
            _navigator = navigator;
            _deviceInfoService = deviceInfoService;
        }

        public async Task StartScanning(Action<DasherFoundEventArgs> onFound)
        {
            if (await _deviceInfoService.GetSelectedInterface() == null)
            {
                await _navigator.DisplayAlertAsync("Error", "Please select network card first.", "Ok");
                return;
            }

            _onDasherFound = onFound;
            _radar = _container.Resolve<IXDashRadar>();
            _radar.DasherFound += onDasherFound;
            await _radar.StartScanning();
        }

        private void onDasherFound(DasherFoundEventArgs e)
        {
            if (_onDasherFound == null)
            {
                return;
            }
            _onDasherFound.Invoke(e);
        }

        public async Task StopScanning()
        {
            if (_radar == null)
            {
                return;
            }
            _radar.DasherFound -= onDasherFound;
            await _radar.StopScanning();
            _radar = null;
        }
    }
}
