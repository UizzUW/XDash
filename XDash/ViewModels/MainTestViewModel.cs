using MVPathway;
using MVPathway.MVVM;
using MVPathway.Presenters.Abstractions;
using System.Windows.Input;
using Xamarin.Forms;

namespace XDash.ViewModels
{
  class MainTestViewModel : BaseViewModel
  {
    private readonly IPresenter mPresenter;

    private ICommand mTestBeaconCommand;
    public ICommand TestBeaconCommand =>
        mTestBeaconCommand ?? (mTestBeaconCommand = new Command(
            () => mPresenter.Show<BeaconTestViewModel>()));

    private ICommand mTestRadarCommand;
    public ICommand TestRadarCommand =>
        mTestRadarCommand ?? (mTestRadarCommand = new Command(
            () => mPresenter.Show<RadarTestViewModel>()));

    private ICommand mShowSettingsCommand;
    public ICommand ShowSettingsCommand =>
        mShowSettingsCommand ?? (mShowSettingsCommand = new Command(
            () => mPresenter.Show<SettingsViewModel>()));

    public MainTestViewModel(IPresenter presenter)
    {
      mPresenter = presenter;
    }
  }
}
