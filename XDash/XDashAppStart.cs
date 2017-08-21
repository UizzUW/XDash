using MVPathway.Builder.Abstractions;
using MVPathway.Navigation.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.ViewModels;

namespace XDash
{
    public class XDashAppStart : IAppStart
    {
        private readonly INavigator _navigator;
        private readonly IDeviceInfoService _deviceInfoService;

        public XDashAppStart(INavigator navigator, IDeviceInfoService deviceInfoService)
        {
            _navigator = navigator;
            _deviceInfoService = deviceInfoService;
        }

        public async void Start()
        {
            await _navigator.Show<SettingsViewModel>();
        }
    }
}
