using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MVPathway.Messages.Abstractions;
using MVPathway.MVVM.Abstractions;
using MVPathway.Navigation.Abstractions;
using Xamarin.Forms;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Components.Transfer.Contracts;
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
        private readonly INavigator _navigator;
        private readonly IXDashFilesystem _filesystem;
        private readonly IDeviceInfoService _deviceInfoService;

        private IXDashScanner _scanner;
        private IXDashBeacon _beacon;
        private IXDashSender _sender;
        private IXDashReceiver _receiver;

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

        public new bool IsBusy
        {
            get => base.IsBusy;
            set
            {
                base.IsBusy = value;
                ScanCommand.ChangeCanExecute();
                OnPropertyChanged();
            }
        }

        private Command _startBeaconCommand;
        public Command StartBeaconCommand =>
            _startBeaconCommand ?? (_startBeaconCommand = new Command(
                async () => await StartListening()));

        private Command _stopBeaconCommand;
        public Command StopBeaconCommand =>
            _stopBeaconCommand ?? (_stopBeaconCommand = new Command(
                async () => await StopListening()));

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
                if (_selectedDasher == null)
                {
                    return;
                }
                PickFileCommand.Execute(null);
            }
        }

        private Command _pickFileCommand;

        public Command PickFileCommand => _pickFileCommand ?? (_pickFileCommand = new Command(
                                              async () => await pickFile()));

        private Command _scanCommand;

        public Command ScanCommand => _scanCommand ?? (_scanCommand = new Command(
                                              async () => await scan(), () => !IsBusy));

        private async Task pickFile()
        {
            string path = await _filesystem.ChooseFile();

            _sender = _container.Resolve<IXDashSender>();
            var dash = new Framework.Models.XDash
            {
                Files = new List<XDashFile>
                {
                    new XDashFile
                    {
                        FullPath = path,
                        Name = path.Substring(path.LastIndexOf(@"\") + 1, path.Length - path.LastIndexOf(@"\") - 1),
                        Size = await _filesystem.GetFileSize(path)
                    }
                }
            };
            var sendResut = await _sender.Send(SelectedDasher, dash);
            if (sendResut.Status != XDashSendResponseStatus.Success)
            {
                await _navigator.DisplayAlertAsync("Error", "Send failed.", "Ok");
                return;
            }
            await _navigator.DisplayAlertAsync("Success", "Gata boss", "Traiasca shefu");
        }

        public DevicesViewModel(IDiContainer container,
                                INavigator navigator,
                                IXDashFilesystem filesystem,
                                IDeviceInfoService deviceInfoService,
                                ILocalizer localizer,
                                IMessenger messenger)
            : base(localizer, messenger)
        {
            _container = container;
            _navigator = navigator;
            _filesystem = filesystem;
            _deviceInfoService = deviceInfoService;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            Dashers = new ObservableCollection<IXDashClient>();
        }

        public async Task StartListening()
        {
            var selectedInterface = await _deviceInfoService.GetSelectedInterface();
            if (selectedInterface == null)
            {
                throw new InvalidOperationException("Please select network card first.");
            }

            _beacon = _container.Resolve<IXDashBeacon>();
            _receiver = _container.Resolve<IXDashReceiver>();
            await _beacon.StartListening();
            await _receiver.StartReceiving(async dash => await Task.FromResult(true));
        }

        public async Task StopListening()
        {
            if (_beacon == null)
            {
                return;
            }
            await _receiver.StopReceiving();
            _receiver = null;
            await _beacon.StopListening();
            _beacon = null;
        }

        private async Task scan()
        {
            var selectedInterface = await _deviceInfoService.GetSelectedInterface();
            if (selectedInterface == null)
            {
                throw new InvalidOperationException("Please select network card first.");
            }

            _scanner = _container.Resolve<IXDashScanner>();
            IsBusy = true;
            var scanResults = await _scanner.Scan();
            IsBusy = false;
            Dashers = new ObservableCollection<IXDashClient>(scanResults);
        }
    }
}
