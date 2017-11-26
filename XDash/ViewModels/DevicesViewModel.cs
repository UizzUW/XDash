using System.Collections.ObjectModel;
using System.Linq;
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
using XDash.ViewModels.ViewObjects;
using BaseViewModel = XDash.ViewModels.Base.BaseViewModel;
using static XDash.Framework.Helpers.ExtensionMethods;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.ViewModels
{
    public class DevicesViewModel : BaseViewModel
    {
        private readonly IDiContainer _container;
        private readonly INavigator _navigator;
        private readonly IFilesystem _filesystem;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IPlatformService _platformService;
        private readonly IXDashClient _client;

        private IScanner _scanner;
        private IBeacon _beacon;
        private IController _sender;
        private IEndpoint _receiver;

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
                OpenSettingsCommand.ChangeCanExecute();
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

        private IXDashClient _selectedDasher;

        public IXDashClient SelectedDasher
        {
            get => _selectedDasher;
            set
            {
                _selectedDasher = value;
                OnPropertyChanged();
            }
        }

        private DashInfoViewObject _dashInfoViewObject;
        public DashInfoViewObject DashInfoViewObject
        {
            get => _dashInfoViewObject;
            set
            {
                _dashInfoViewObject = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasIncomingDash));
            }
        }

        public bool HasIncomingDash => DashInfoViewObject != null;

        public Command<IXDashClient> SendFilesCommand => new Command<IXDashClient>(
                async client => await onSendFiles(client));

        private Command _scanCommand;
        public Command ScanCommand => _scanCommand ?? (_scanCommand = new Command(
                                              async () => await scan(), () => !IsBusy));


        private Command _openSettingsCommand;
        public Command OpenSettingsCommand => _openSettingsCommand ?? (_openSettingsCommand = new Command(
                                          async () => await _navigator.Show<SettingsViewModel>(), () => !IsBusy));

        private async Task onSendFiles(IXDashClient client)
        {
            var paths = await _filesystem.ChooseFiles();
            if (paths == null || !paths.Any())
            {
                return;
            }
            var files = paths.Select(async p => new XDashFile
            {
                FullPath = p,
                Name = Device.RuntimePlatform == "Android"
                ? p.Substring(p.LastIndexOf(@"/") + 1, p.Length - p.LastIndexOf(@"/") - 1)
                : p.Substring(p.LastIndexOf(@"\") + 1, p.Length - p.LastIndexOf(@"\") - 1),
                Size = await _filesystem.GetFileSize(p)
            }).ToList();

            await Task.WhenAll(files);

            _sender = _container.Resolve<IController>();
            var dash = new Framework.Models.XDash
            {
                Sender = client as XDashClient,
                Files = files.Select(f => f.Result).ToArray()
            };
            var sendResut = await _sender.Send(client, dash);
            if (sendResut.Status != XDashSendResponseStatus.Success)
            {
                await _navigator.DisplayAlertAsync(this["Error"], this["Send_Failed"], this["Ok"]);
                return;
            }
            await _navigator.DisplayAlertAsync(this["Success"], this["Send_Successful"], this["Ok"]);
        }

        public DevicesViewModel(IDiContainer container,
                                INavigator navigator,
                                IFilesystem filesystem,
                                IDeviceInfoService deviceInfoService,
                                IPlatformService platformService,
                                IXDashClient client,
                                ILocalizer localizer,
                                IMessenger messenger)
                    : base(localizer, messenger)
        {
            _container = container;
            _navigator = navigator;
            _filesystem = filesystem;
            _deviceInfoService = deviceInfoService;
            _platformService = platformService;
            _client = client;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);

            var selectedInterface = await _deviceInfoService.GetSelectedInterface();
            if (selectedInterface == null)
            {
                var result = await _navigator.DisplayAlertAsync("No network interface selected",
                    "Please select a network interface first.", "Configure", "Exit app");
                if (result)
                {
                    await _navigator.Show<SettingsViewModel>();
                }
                else
                {
                    _platformService.ExitApp();
                }
                return;
            }

            //Dashers = new ObservableCollection<IXDashClient>();
            OnPropertyChanged(nameof(HasIncomingDash));
        }

        public async Task StartListening()
        {
            _beacon = _container.Resolve<IBeacon>();
            _receiver = _container.Resolve<IEndpoint>();
            await _beacon.StartListening();
            await _receiver.StartReceiving(async dash => await onDashReceived(dash),
                async result => await TaskOnUiThread(async () => await _navigator.DisplayAlertAsync(result ? this["Success"] : this["Error"],
                result ? this["Receive_Successful"] : this["Receive_Failed"], this["Ok"])));
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

        private async Task<bool> onDashReceived(Framework.Models.XDash dash)
        {
            DashInfoViewObject = new DashInfoViewObject(Localizer, dash);
            var result = await DashInfoViewObject.GetResult();
            DashInfoViewObject = null;
            return result;
        }

        private async Task scan()
        {
            _scanner = _container.Resolve<IScanner>();
            IsBusy = true;
            var scanResults = await _scanner.Scan();
            IsBusy = false;
            Dashers = new ObservableCollection<IXDashClient>(scanResults);
        }
    }
}
