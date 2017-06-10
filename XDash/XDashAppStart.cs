using MVPathway.Builder.Abstractions;
using MVPathway.MVVM.Abstractions;
using MVPathway.Navigation.Abstractions;
using XDash.Framework.Services.Contracts;
using XDash.Pages;
using XDash.ViewModels;

namespace XDash
{
    public class XDashAppStart : IAppStart
    {
        private readonly IDiContainer _container;
        private readonly INavigator _navigator;
        private readonly IViewModelManager _vmManager;
        private readonly IDeviceInfoService _deviceInfoService;

        public XDashAppStart(IDiContainer container,
                             INavigator navigator,
                             IViewModelManager vmManager,
                             IDeviceInfoService deviceInfoService)
        {
            _container = container;
            _navigator = navigator;
            _vmManager = vmManager;
            _deviceInfoService = deviceInfoService;
        }

        public async void Start()
        {
            _vmManager.RegisterPageForViewModel<MainTestViewModel, MainTestPage>();
            _vmManager.RegisterPageForViewModel<BeaconTestViewModel, BeaconTestPage>();
            _vmManager.RegisterPageForViewModel<RadarTestViewModel, RadarTestPage>();
            _vmManager.RegisterPageForViewModel<SettingsViewModel, SettingsPage>();

            await _deviceInfoService.Init();

            await _navigator.Show<MainTestViewModel>();
        }
    }
}
