using XDash.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MVPathway.Messages.Abstractions;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Services.Contracts;
using XDash.Framework.Configuration.Contracts;
using System.Net.NetworkInformation;
using XDash.Framework.Helpers;

namespace XDash.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IConfigurator _configurator;

        private IXDashClient _client;

        public IXDashClient Client
        {
            get => _client;
            set
            {
                _client = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Language> Languages
            => new ObservableCollection<Language>(Localizer.SupportedLanguages);

        public Language SelectedLanguage
        {
            get => Localizer.CurrentLanguage;
            set
            {
                Localizer.CurrentLanguage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NetworkInterface> _interfaces;
        public ObservableCollection<NetworkInterface> Interfaces
        {
            get => _interfaces;
            set
            {
                _interfaces = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedInterface));
            }
        }

        private NetworkInterface _selectedInterface;
        public NetworkInterface SelectedInterface
        {
            get => _selectedInterface;
            set
            {
                _selectedInterface = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel(IConfigurator configurator,
                                 IDeviceInfoService deviceInfoService,
                                 IXDashClient client,
                                 ILocalizer localizer,
                                 IMessenger messenger)
            : base(localizer, messenger)
        {
            _configurator = configurator;
            _deviceInfoService = deviceInfoService;
            Client = client;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            SelectedInterface = _deviceInfoService.GetSelectedInterface();
            Interfaces = new ObservableCollection<NetworkInterface>(_deviceInfoService.GetInterfaces());
        }

        protected override async Task OnNavigatingFrom(object parameter)
        {
            await base.OnNavigatingFrom(parameter);
            await _deviceInfoService.SetSelectedInterface(SelectedInterface);

            var options = _configurator.GetConfiguration();

            options.Device.Name = Client.Name;
            options.Device.Ip = SelectedInterface?.GetValidIPv4();
            options.Device.Language = SelectedLanguage;

            await _configurator.SaveConfiguration(options);
        }
    }
}
