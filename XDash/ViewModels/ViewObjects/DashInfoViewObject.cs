using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVPathway.MVVM.Abstractions;
using Xamarin.Forms;
using XDash.Framework.Helpers;

namespace XDash.ViewModels.ViewObjects
{
    public class DashInfoViewObject : BaseObservableObject
    {
        private Framework.Models.XDash _dash;
        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        public string Text { get; }

        private Command _acceptCommand;
        public Command AcceptCommand => _acceptCommand ?? (_acceptCommand = new Command(() => _tcs.SetResult(true)));

        private Command _declineCommand;
        public Command DeclineCommand => _declineCommand ?? (_declineCommand = new Command(() => _tcs.SetResult(false)));

        public DashInfoViewObject(Framework.Models.XDash dash)
        {
            _dash = dash;
            var messageBuilder = new StringBuilder();
            messageBuilder.Append($"Sender : {dash.Sender.Name}\n");
            messageBuilder.Append($"Total size : {dash.Files.Select(f => f.Size).Aggregate((prev, next) => prev + next).AsFormattedBytesValue()}\n\n");
            messageBuilder.Append("Files : \n");
            foreach (var file in dash.Files)
            {
                messageBuilder.Append($"{file.Name} ({file.Size.AsFormattedBytesValue()})\n");
            }
            Text = $"Incoming Dash : \n\n{messageBuilder}";
        }

        public async Task<bool> GetResult() => await _tcs.Task;
    }
}
