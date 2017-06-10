using MVPathway.MVVM.Abstractions;
using Sockets.Plugin.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XDash.Framework.Models;
using XDash.Framework.Services.Contracts;

namespace XDash.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IDeviceInfoService _deviceInfoService;

        public XDashClient DeviceInfo
        {
            get
            {
                return _deviceInfoService.GetDeviceInfo();
            }
        }

        public string DeviceName
        {
            get
            {
                return DeviceInfo.Name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                _deviceInfoService.RenameDevice(value);
            }
        }

        private ObservableCollection<ICommsInterface> mInterfaces;
        public ObservableCollection<ICommsInterface> Interfaces
        {
            get
            {
                return mInterfaces;
            }
            set
            {
                mInterfaces = value;
                OnPropertyChanged(nameof(Interfaces));
                OnPropertyChanged(nameof(SelectedInterface));
            }
        }

        public ICommsInterface SelectedInterface
        {
            get
            {
                return _deviceInfoService.SelectedInterface;
            }
            set
            {
                _deviceInfoService.SelectedInterface = value;
                OnPropertyChanged(nameof(SelectedInterface));
            }
        }

        public SettingsViewModel(IDeviceInfoService deviceInfoService)
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
