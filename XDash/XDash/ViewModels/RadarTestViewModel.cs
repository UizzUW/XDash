using MVPathway.MVVM;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Models;
using XDash.Services.Contracts;

namespace XDash.ViewModels
{
    class RadarTestViewModel : BaseViewModel
    {
        private readonly IRadarService mRadarService;

        public ObservableCollection<XDashClient> Dashers { get; set; }

        public RadarTestViewModel(IRadarService radarService)
        {
            mRadarService = radarService;
            Dashers = new ObservableCollection<XDashClient>();
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            await mRadarService.StartScanning(onDasherFound);
        }

        protected override async Task OnNavigatingFrom(object parameter)
        {
            await base.OnNavigatingFrom(parameter);
            await mRadarService.StopScanning();
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
