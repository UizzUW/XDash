using MVPathway;
using MVPathway.Builder.Abstractions;
using MVPathway.Utils.Presenters;
using Xamarin.Forms;
using XDash.Framework.Builder;

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
              .UsePresenter<StackPresenter>()
              .UseAppStart<XDashAppStart>()
              .UseXDash();
        }
    }
}
