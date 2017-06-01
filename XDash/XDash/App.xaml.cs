
using MVPathway;
using MVPathway.Builder.Abstractions;
using MVPathway.Logging.Abstractions;
using MVPathway.Messages.Abstractions;
using MVPathway.MVVM.Abstractions;
using MVPathway.Presenters.Abstractions;
using Xamarin.Forms.Xaml;
using XDash.Pages;
using XDash.Services;
using XDash.Services.Contracts;
using XDash.ViewModels;
using MVPathway.Utils.Presenters;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XDash
{
  public partial class App : PathwayApplication
  {
    public App()
    {
      InitializeComponent();
    }

    public override void Configure(IPathwayBuilder builder)
    {
      base.Configure(builder);
      builder.UsePresenter<NavigationPagePresenter>();
    }

    public override void Init(IDiContainer container,
                              IViewModelManager vmManager,
                              IMessagingManager messagingManager,
                              IPresenter presenter,
                              ILogger logger)
    {
      container.Register<IObjectSerializerService, JsonSerializerService>();
      container.Register<ISettingsService, SettingsService>();
      container.Register<IDeviceInfoService, DeviceInfoService>();
      container.Register<IBeaconService, XDashBeaconService>();
      container.Register<IRadarService, XDashRadarService>();

      vmManager.RegisterPageForViewModel<MainTestViewModel, MainTestPage>();
      vmManager.RegisterPageForViewModel<BeaconTestViewModel, BeaconTestPage>();
      vmManager.RegisterPageForViewModel<RadarTestViewModel, RadarTestPage>();
      vmManager.RegisterPageForViewModel<SettingsViewModel, SettingsPage>();

      container.Resolve<IDeviceInfoService>().Init();

      presenter.Show<MainTestViewModel>();
    }
  }
}
