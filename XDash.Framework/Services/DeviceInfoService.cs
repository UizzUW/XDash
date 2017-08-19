using System;
using System.Linq;
using System.Reflection;
using XDash.Framework.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts;
using Sockets.Plugin;
using Sockets.Plugin.Abstractions;

namespace XDash.Framework.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private const string DEVICE_NAME_KEY = nameof(DEVICE_NAME_KEY);
        private const string DEVICE_GUID_KEY = nameof(DEVICE_GUID_KEY);
        private const string DEVICE_INFO_KEY = nameof(DEVICE_INFO_KEY);
        private const string SELECTED_COMMS_INTERFACE_KEY = nameof(SELECTED_COMMS_INTERFACE_KEY);

        private readonly ISettingsRepository _settingsService;
        private readonly IPlatformService _platformService;

        public IEnumerable<ICommsInterface> Interfaces { get; private set; }

        private ICommsInterface _selectedInterface;
        public ICommsInterface SelectedInterface
        {
            get
            {
                if (_selectedInterface != null)
                {
                    return _selectedInterface;
                }

                var ip = _settingsService.Get<string>(SELECTED_COMMS_INTERFACE_KEY);
                _selectedInterface = Interfaces?.FirstOrDefault(x => x.IpAddress == ip);
                return _selectedInterface;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                _selectedInterface = value;
                if (!Interfaces.Any(x => x.IpAddress == _selectedInterface.IpAddress))
                {
                    throw new ArgumentException("This interface is not available on this device.");
                }
                _settingsService.Set(SELECTED_COMMS_INTERFACE_KEY, _selectedInterface.IpAddress);
            }
        }

        public DeviceInfoService(ISettingsRepository settingsService, IPlatformService platformService)
        {
            _settingsService = settingsService;
            _platformService = platformService;
        }

        public async Task Init()
        {
            Interfaces = (await CommsInterface.GetAllInterfacesAsync())
                .Where(x => x.IsUsable && !x.IsLoopback)
                .OrderBy(x => x.IpAddress)
                .ToList();
        }

        public XDashClient GetDeviceInfo()
        {
            return new XDashClient
            {
                Guid = getGuid(),
                Name = getName(),
                Ip = SelectedInterface?.IpAddress,
                OperatingSystem = _platformService.GetOperatingSystem(),
                FrameworkVersion = new AssemblyName(typeof(XDashClient)
                    .GetTypeInfo()
                    .Assembly.FullName)
                    .Version.ToString()
            };
        }

        public void RenameDevice(string name) => _settingsService.Set(DEVICE_NAME_KEY, name);

        private string getGuid()
        {
            var guid = _settingsService.Get<Guid>(DEVICE_GUID_KEY);
            if (guid != Guid.Empty)
            {
                return guid.ToString();
            }

            guid = Guid.NewGuid();
            _settingsService.Set(DEVICE_GUID_KEY, guid);
            return guid.ToString();
        }

        private string getName()
        {
            var name = _settingsService.Get<string>(DEVICE_NAME_KEY);
            if (name != null)
            {
                return name;
            }

            name = $"{_platformService.GetOperatingSystem()} Dasher";
            RenameDevice(name);
            return name;
        }
    }
}
