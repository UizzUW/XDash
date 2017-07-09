using MVPathway.MVVM.Abstractions;
using MVPathway.Navigation;
using MVPathway.Navigation.Abstractions;
using MVPathway.Utils.Presenters;
using MVPathway.Utils.ViewModels.Qualities;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XDash.Helpers
{
    public class CustomTabbedPresenter : StackPresenter<NavigationPage>
    {
        private readonly IViewModelManager _vmManager;

        protected TabbedPage TabbedPage { get; set; }

        public CustomTabbedPresenter(IViewModelManager vmManager,
                                     INavigator navigator)
            : base(navigator)
        {
            _vmManager = vmManager;
        }

        public override async Task Init()
        {
            await base.Init();
            TabbedPage = new TabbedPage();
            TabbedPage.CurrentPageChanged += async (s, e) => await onTabChanged(s, e);
            var childPages = _vmManager.ResolvePagesForViewModels(def => def.HasQuality<IChildQuality>());
            foreach (var child in childPages)
            {
                TabbedPage.Children.Add(child);
                NavigationPage.SetHasNavigationBar(child, false);
            }
        }

        public override async Task OnShow(BaseViewModel viewModel, Page page, NavigationRequestType requestType)
        {
            if (viewModel.Definition.HasQuality<IChildQuality>())
            {
                TabbedPage.CurrentPage = page;
                TabbedPage.Title = page.Title;

                await base.OnShow(viewModel, TabbedPage, requestType);
                return;
            }
            await base.OnShow(viewModel, page, requestType);
        }

        private async Task onTabChanged(object sender, EventArgs e)
        {
            if (Navigator.DuringRequestedTransition)
            {
                return;
            }
            var newVm = TabbedPage.CurrentPage.BindingContext as BaseViewModel;
            if (newVm == null)
            {
                return;
            }
            await Navigator.Show(newVm);
        }
    }
}
