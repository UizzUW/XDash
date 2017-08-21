using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MVPathway.Messages.Abstractions;
using MVPathway.MVVM.Abstractions;
using Plugin.FilePicker;
using Xamarin.Forms;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Services.Contracts;
using BaseViewModel = XDash.ViewModels.Base.BaseViewModel;

namespace XDash.ViewModels
{
    public class DevicesViewModel : BaseViewModel
    {
        private readonly IDiContainer _container;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IXDashClient _client;
        private IXDashScanner _radar;
        private IXDashBeacon _beacon;

        private bool _isBeaconEnabled;
        public bool IsBeaconEnabled
        {
            get => _isBeaconEnabled;
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
                async () => await StartBroadcasting()));

        private Command _stopBeaconCommand;
        public Command StopBeaconCommand =>
            _stopBeaconCommand ?? (_stopBeaconCommand = new Command(
                async () => await StopBroadcasting()));

        private ObservableCollection<IXDashClient> _dashers;
        public ObservableCollection<IXDashClient> Dashers
        {
            get => _dashers;
            set
            {
                _dashers = value;
                OnPropertyChanged();
            }
        }

        private XDashClient _selectedDasher;

        public XDashClient SelectedDasher
        {
            get => _selectedDasher;
            set
            {
                _selectedDasher = value;
                PickFileCommand.Execute(null);
            }
        }

        private Command _pickFileCommand;

        public Command PickFileCommand => _pickFileCommand ?? (_pickFileCommand = new Command(
                                              async () => await pickFile()));

        private async Task pickFile()
        {
            var f = await CrossFilePicker.Current.PickFile();
            CrossFilePicker.Current.OpenFile(f);
        }

        public DevicesViewModel(IDiContainer container,
                                IDeviceInfoService deviceInfoService,
                                IXDashClient client,
                                ILocalizer localizer,
                                IMessenger messenger)
            : base(localizer, messenger)
        {
            _container = container;
            _deviceInfoService = deviceInfoService;
            _client = client;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            Dashers = new ObservableCollection<IXDashClient>();
            var selectedInterface = await _deviceInfoService.GetSelectedInterface();
            if (selectedInterface == null)
            {
                throw new InvalidOperationException("Please select network card first.");
            }

            _radar = _container.Resolve<IXDashScanner>();
            _radar.DasherFound += onDasherFound;
            _radar.Client = _client;
            _radar.Interface = selectedInterface;
            _radar.DasherFound += onDasherFound;
            await _radar.StartScanning();
        }

        protected override async Task OnNavigatingFrom(object parameter)
        {
            await base.OnNavigatingFrom(parameter);
            if (_radar == null)
            {
                return;
            }
            _radar.DasherFound -= onDasherFound;
            await _radar.StopScanning();
            _radar = null;
        }

        public async Task StartBroadcasting()
        {
            var selectedInterface = await _deviceInfoService.GetSelectedInterface();
            if (selectedInterface == null)
            {
                throw new InvalidOperationException("Please select network card first.");
            }

            _beacon = _container.Resolve<IXDashBeacon>();
            _beacon.Client = _client;
            _beacon.Interface = selectedInterface;
            _beacon.StartBroadcasting();
        }

        public async Task StopBroadcasting()
        {
            if (_beacon == null)
            {
                return;
            }
            await _beacon.StopBroadcasting();
            _beacon = null;
        }

        private void onDasherFound(DasherFoundEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (e.IsBroadcasting && !Dashers.Contains(e.RemoteDeviceClient))
                {
                    Dashers.Add(e.RemoteDeviceClient);
                }
                else if (!e.IsBroadcasting && Dashers.Contains(e.RemoteDeviceClient))
                {
                    Dashers.Remove(e.RemoteDeviceClient);
                }
            });
        }
    }
}
