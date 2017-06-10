using MVPathway.MVVM.Abstractions;
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
        private readonly IDeviceInfoService _deviceInfoService;
        private IXDashRadar _radar;

        private Action<DasherFoundEventArgs> _onDasherFound;

        public XDashRadarService(IDiContainer container, IDeviceInfoService deviceInfoService)
        {
            _container = container;
            _deviceInfoService = deviceInfoService;
        }

        public async Task StartScanning(Action<DasherFoundEventArgs> onFound)
        {
            _onDasherFound = onFound;
            _radar = _container.Resolve<IXDashRadar>();
            _radar.Client = _deviceInfoService.GetDeviceInfo();
            _radar.Interface = _deviceInfoService.SelectedInterface;
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
            await _radar.StopScanning();
        }
    }
}
