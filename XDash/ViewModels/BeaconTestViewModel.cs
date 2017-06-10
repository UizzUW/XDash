using MVPathway.MVVM.Abstractions;
using Xamarin.Forms;
using XDash.Framework.Services.Contracts;

namespace XDash.ViewModels
{
    class BeaconTestViewModel : BaseViewModel
    {
        private readonly IBeaconService _beaconService;

        private Command _startBeaconCommand;
        public Command StartBeaconCommand =>
            _startBeaconCommand ?? (_startBeaconCommand = new Command(
                async () => await _beaconService.StartBroadcasting()));

        private Command _stopBeaconCommand;
        public Command StopBeaconCommand =>
            _stopBeaconCommand ?? (_stopBeaconCommand = new Command(
                () => _beaconService.StopBroadcasting()));

        public BeaconTestViewModel(IBeaconService beaconService)
        {
            _beaconService = beaconService;
        }
    }
}
