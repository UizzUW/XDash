using MVPathway.Builder.Abstractions;
using MVPathway.Navigation.Abstractions;
using XDash.ViewModels;

namespace XDash
{
    public class XDashAppStart : IAppStart
    {
        private readonly INavigator _navigator;

        public XDashAppStart(INavigator navigator)
        {
            _navigator = navigator;
        }

        public async void Start()
        {
            await _navigator.Show<DevicesViewModel>();
        }
    }
}
