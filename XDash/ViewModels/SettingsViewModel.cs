using XDash.ViewModels.Base;
using Sockets.Plugin.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MVPathway.Messages.Abstractions;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;
using XDash.Models.Enums;
using XDash.Services.Contracts;

namespace XDash.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDeviceInfoService _deviceInfoService;

        public XDashClient DeviceInfo => _deviceInfoService.GetDeviceInfo();

        public string DeviceName
        {
            get => DeviceInfo.Name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                _deviceInfoService.RenameDevice(value);
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

        public ICommsInterface SelectedInterface
        {
            get => _deviceInfoService.SelectedInterface;
            set
            {
                _deviceInfoService.SelectedInterface = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel(IDeviceInfoService deviceInfoService,
                                 ILocalizer localizer,
                                 IMessenger messenger)
            : base(localizer, messenger)
        {
            _deviceInfoService = deviceInfoService;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            Interfaces = new ObservableCollection<ICommsInterface>(_deviceInfoService.Interfaces);
        }
    }
}
