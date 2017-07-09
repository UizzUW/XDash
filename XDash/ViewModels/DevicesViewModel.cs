using MVPathway.MVVM.Abstractions;
using Plugin.FilePicker;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.ViewModels
{
    public class DevicesViewModel : BaseViewModel
    {
        private readonly IRadarService _radarService;
        private readonly IBeaconService _beaconService;

        private bool _isBeaconEnabled;
        public bool IsBeaconEnabled
        {
            get { return _isBeaconEnabled; }
            set
            {
                _isBeaconEnabled = value;
                if (_isBeaconEnabled)
                {
                    StartBeaconCommand.Execute(null);
                }
                else
                {
                    StopBeaconCommand.Execute(null);
                }
                OnPropertyChanged();
            }
        }

        private Command _startBeaconCommand;
        public Command StartBeaconCommand =>
            _startBeaconCommand ?? (_startBeaconCommand = new Command(
                async () => await _beaconService.StartBroadcasting()));

        private Command _stopBeaconCommand;
        public Command StopBeaconCommand =>
            _stopBeaconCommand ?? (_stopBeaconCommand = new Command(
                async () => await _beaconService.StopBroadcasting()));

        private ObservableCollection<XDashClient> _dashers;
        public ObservableCollection<XDashClient> Dashers
        {
            get => _dashers;
            set
            {
                _dashers = value;
                OnPropertyChanged();
            }
        }

        public DevicesViewModel(IBeaconService beaconService,
                                  IRadarService radarService)
        {
            _beaconService = beaconService;
            _radarService = radarService;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            Dashers = new ObservableCollection<XDashClient>();
            await _radarService.StartScanning(onDasherFound);
        }

        protected override async Task OnNavigatingFrom(object parameter)
        {
            await base.OnNavigatingFrom(parameter);
            await _radarService.StopScanning();
        }

        private void onDasherFound(DasherFoundEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (e.IsBroadcasting && !Dashers.Contains(e.RemoteDeviceClientInfo))
                {
                    Dashers.Add(e.RemoteDeviceClientInfo);
                }
                else if (!e.IsBroadcasting && Dashers.Contains(e.RemoteDeviceClientInfo))
                {
                    Dashers.Remove(e.RemoteDeviceClientInfo);
                }
            });
        }
    }
}
