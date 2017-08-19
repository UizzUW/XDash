using System.ServiceModel.Channels;
using System.Threading.Tasks;
using MVPathway.Messages.Abstractions;
using XDash.Messages;
using XDash.Services.Contracts;

namespace XDash.ViewModels.Base
{
    public class BaseViewModel : MVPathway.MVVM.Abstractions.BaseViewModel
    {
        protected ILocalizer Localizer { get; }
        protected IMessenger Messenger { get; }

        public string this[string key] => Localizer.GetLocalizedString(key);

        public BaseViewModel(ILocalizer localizer, IMessenger messenger)
        {
            Localizer = localizer;
            Messenger = messenger;
        }

        protected override async Task OnNavigatedTo(object parameter)
        {
            await base.OnNavigatedTo(parameter);
            Messenger.Subscribe<LanguageChangedMessage>(m =>
            {
                OnPropertyChanged("Item");
            });
        }
    }
}
