using MVPathway.Builder;
using XDash.Services.Contracts;
using XDash.UWP.Services;

namespace XDash.UWP
{
  public sealed partial class MainPage
  {
    public MainPage()
    {
      this.InitializeComponent();

      LoadApplication(PathwayFactory.Create<XDash.App>(di =>
      {
        di.Register<IPlatformService, WindowsPlatformService>();
      }));
    }
  }
}
