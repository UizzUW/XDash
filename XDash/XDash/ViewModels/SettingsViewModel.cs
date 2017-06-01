using MVPathway.MVVM;
using Sockets.Plugin.Abstractions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XDash.Framework.Models;
using XDash.Services.Contracts;

namespace XDash.ViewModels
{
  public class SettingsViewModel : BaseViewModel
  {
    private readonly IDeviceInfoService mDeviceInfoService;

    public XDashClient DeviceInfo
    {
      get
      {
        return mDeviceInfoService.GetDeviceInfo();
      }
    }

    public string DeviceName
    {
      get
      {
        return DeviceInfo.Name;
      }
      set
      {
        if (string.IsNullOrWhiteSpace(value))
        {
          return;
        }
        mDeviceInfoService.RenameDevice(value);
      }
    }

    private ObservableCollection<ICommsInterface> mInterfaces;
    public ObservableCollection<ICommsInterface> Interfaces
    {
      get
      {
        return mInterfaces;
      }
      set
      {
        mInterfaces = value;
        OnPropertyChanged(nameof(Interfaces));
        OnPropertyChanged(nameof(SelectedInterface));
      }
    }

    public ICommsInterface SelectedInterface
    {
      get
      {
        return mDeviceInfoService.SelectedInterface;
      }
      set
      {
        mDeviceInfoService.SelectedInterface = value;
        OnPropertyChanged(nameof(SelectedInterface));
      }
    }

    public SettingsViewModel(IDeviceInfoService deviceInfoService)
    {
      mDeviceInfoService = deviceInfoService;
    }

    protected override async Task OnNavigatedTo(object parameter)
    {
      await base.OnNavigatedTo(parameter);
      Interfaces = new ObservableCollection<ICommsInterface>(mDeviceInfoService.Interfaces);
    }
  }
}
