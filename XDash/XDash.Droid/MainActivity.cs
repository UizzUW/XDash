
using Android.App;
using Android.Content.PM;
using Android.OS;
using MVPathway.Builder;
using XDash.Services.Contracts;
using XDash.Droid.Services;

namespace XDash.Droid
{
  [Activity(Label = "XamarinForms.NetStandard", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      base.OnCreate(bundle);

      global::Xamarin.Forms.Forms.Init(this, bundle);
      LoadApplication(PathwayFactory.Create<App>(di =>
      {
        di.Register<IPlatformService, AndroidPlatformService>();
      }));
    }
  }
}

