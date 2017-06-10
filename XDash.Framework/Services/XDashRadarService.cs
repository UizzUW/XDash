using System;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;
using XDash.Services.Contracts;

namespace XDash.Services
{
    class XDashRadarService : IRadarService
    {
        private XDashRadar _radar;
        private Action<DasherFoundEventArgs> _onDasherFound;
        private readonly IDeviceInfoService _deviceInfoService;

        public XDashRadarService(IDeviceInfoService deviceInfoService)
        {
            _deviceInfoService = deviceInfoService;
        }

        public async Task StartScanning(Action<DasherFoundEventArgs> onFound)
        {
            _onDasherFound = onFound;
            _radar = new XDashRadar
            {
                Client = _deviceInfoService.GetDeviceInfo(),
                Interface = _deviceInfoService.SelectedInterface
            };
            _radar.DasherFound += onDasherFound;
            await _radar.StartScanning();
        }

        private void onDasherFound(DasherFoundEventArgs e)
        {
            if(_onDasherFound == null)
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
