using MVPathway;
using MVPathway.Builder.Abstractions;
using MVPathway.Utils.Presenters;
using Xamarin.Forms;
using XDash.Framework.Builder;
using XDash.Helpers;

namespace XDash
{
    public partial class App : PathwayApplication
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
        }

        public override void Configure(IPathwayBuilder builder)
        {
            base.Configure(builder);
            builder
              .UsePresenter<CustomTabbedPresenter>()
              .UseAppStart<XDashAppStart>()
              .UseXDash();
        }
    }
}
