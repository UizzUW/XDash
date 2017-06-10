using MVPathway.MVVM;
using System.Windows.Input;
using Xamarin.Forms;
using XDash.Services.Contracts;

namespace XDash.ViewModels
{
    class BeaconTestViewModel : BaseViewModel
    {
        private readonly IBeaconService mBeaconService;

        private Command mStartBeaconCommand;
        public Command StartBeaconCommand =>
            mStartBeaconCommand ?? (mStartBeaconCommand = new Command(
                async () => await mBeaconService.StartBroadcasting()));

        private Command mStopBeaconCommand;
        public Command StopBeaconCommand =>
            mStopBeaconCommand ?? (mStopBeaconCommand = new Command(
                () => mBeaconService.StopBroadcasting()));

        public BeaconTestViewModel(IBeaconService beaconService)
        {
            mBeaconService = beaconService;
        }
    }
}
