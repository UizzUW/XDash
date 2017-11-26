using XDash.ViewModels.Base;
using Sockets.Plugin.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MVPathway.Messages.Abstractions;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Services.Contracts;
using XDash.Framework.Configuration.Contracts;

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

        private ObservableCollection<ICommsInterface> _interfaces;
        public ObservableCollection<ICommsInterface> Interfaces
        {
            get => _interfaces;
            set
            {
                _interfaces = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedInterface));
            }
        }

        private ICommsInterface _selectedInterface;
        public ICommsInterface SelectedInterface
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
            SelectedInterface = await _deviceInfoService.GetSelectedInterface();
            Interfaces = new ObservableCollection<ICommsInterface>(await _deviceInfoService.GetInterfaces());
        }

        protected override async Task OnNavigatingFrom(object parameter)
        {
            await base.OnNavigatingFrom(parameter);
            await _deviceInfoService.SetSelectedInterface(SelectedInterface);

            var options = _configurator.GetConfiguration();

            options.Device.Name = Client.Name;
            options.Device.Ip = SelectedInterface?.IpAddress;
            options.Device.Language = SelectedLanguage;

            await _configurator.SaveConfiguration(options);
        }
    }
}
