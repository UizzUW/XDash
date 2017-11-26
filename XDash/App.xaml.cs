using DryIoc;
using MVPathway.Builder.Abstractions;
using MVPathway.MVVM.Abstractions;
using MVPathway.Utils.Presenters;
using Xamarin.Forms;
using XDash.Framework.Emvy.Builder;
using XDash.Services;
using XDash.Services.Contracts;

namespace XDash
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
        }


        public override void Configure(IPathwayBuilder builder)
        {
            base.Configure(builder);

            Container.Register<ILocalizer, Localizer>();
            Container.Resolve<IViewModelManager>().AutoScanAndRegister(GetType().GetAssembly());

            builder
                .UsePresenter<StackPresenter>()
                .UseAppStart<XDashAppStart>()
                .UseXDash();
        }
    }
}
