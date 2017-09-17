using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVPathway.MVVM.Abstractions;
using Xamarin.Forms;
using XDash.Framework.Helpers;
using XDash.Services.Contracts;

namespace XDash.ViewModels.ViewObjects
{
    public class DashInfoViewObject : BaseObservableObject
    {
        private readonly ILocalizer _localizer;

        public string this[string key] => _localizer.GetLocalizedString(key);

        private Framework.Models.XDash _dash;
        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public string Text { get; }

        private Command _acceptCommand;
        public Command AcceptCommand => _acceptCommand ?? (_acceptCommand = new Command(() => _tcs.SetResult(true)));

        private Command _declineCommand;
        public Command DeclineCommand => _declineCommand ?? (_declineCommand = new Command(() => _tcs.SetResult(false)));

        public DashInfoViewObject(ILocalizer localizer, Framework.Models.XDash dash)
        {
            _localizer = localizer;
            _dash = dash;
            var messageBuilder = new StringBuilder();
            messageBuilder.Append($"{this["Sender"]} : {dash.Sender.Name}\n");
            messageBuilder.Append($"{this["Total_Size"]} : {dash.Files.Select(f => f.Size).Aggregate((prev, next) => prev + next).AsFormattedBytesValue()}\n\n");
            messageBuilder.Append($"{this["Files"]} ({dash.Files.Length}) : \n");
            foreach (var file in dash.Files)
            {
                messageBuilder.Append($"{file.Name} ({file.Size.AsFormattedBytesValue()})\n");
            }
            Text = $"{this["Incoming_Dash"]} : \n\n{messageBuilder}";
        }

        public async Task<bool> GetResult() => await _tcs.Task;
    }
}
