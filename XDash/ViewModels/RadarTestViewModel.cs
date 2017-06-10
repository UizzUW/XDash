using MVPathway.MVVM.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.ViewModels
{
    class RadarTestViewModel : BaseViewModel
    {
        private readonly IRadarService _radarService;

        public ObservableCollection<XDashClient> Dashers { get; set; }

        public RadarTestViewModel(IRadarService radarService)
        {
            _radarService = radarService;
            Dashers = new ObservableCollection<XDashClient>();
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            await _radarService.StartScanning(onDasherFound);
        }

        protected override async Task OnNavigatingFrom(object parameter)
        {
            await base.OnNavigatingFrom(parameter);
            await _radarService.StopScanning();
        }

        private void onDasherFound(DasherFoundEventArgs e)
        {
            if (!Dashers.Contains(e.RemoteDeviceClientInfo))
            {
                Dashers.Add(e.RemoteDeviceClientInfo);
            }
        }
    }
}
