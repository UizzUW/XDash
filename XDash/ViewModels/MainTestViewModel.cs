using MVPathway.MVVM.Abstractions;
using MVPathway.Navigation.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;

namespace XDash.ViewModels
{
    class MainTestViewModel : BaseViewModel
    {
        private readonly INavigator _navigator;

        private ICommand mTestBeaconCommand;
        public ICommand TestBeaconCommand =>
            mTestBeaconCommand ?? (mTestBeaconCommand = new Command(
                () => _navigator.Show<BeaconTestViewModel>()));

        private ICommand mTestRadarCommand;
        public ICommand TestRadarCommand =>
            mTestRadarCommand ?? (mTestRadarCommand = new Command(
                () => _navigator.Show<RadarTestViewModel>()));

        private ICommand mShowSettingsCommand;
        public ICommand ShowSettingsCommand =>
            mShowSettingsCommand ?? (mShowSettingsCommand = new Command(
                () => _navigator.Show<SettingsViewModel>()));

        public MainTestViewModel(INavigator navigator)
        {
            _navigator = navigator;
        }
    }
}
