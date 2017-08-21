using System;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class SettingsRepository : MVPathway.Settings.SettingsRepository, ISettingsRepository
    {
        private const string _DEVICE_NAME_KEY = nameof(_DEVICE_NAME_KEY);
        private const string _DEVICE_GUID_KEY = nameof(_DEVICE_GUID_KEY);
        private const string _SELECTED_COMMS_INTERFACE_KEY = nameof(_SELECTED_COMMS_INTERFACE_KEY);
        private const string _BEACON_SCAN_PORT = nameof(_BEACON_SCAN_PORT);
        private const string _SCAN_RESPONSE_PORT = nameof(_SCAN_RESPONSE_PORT);

        public Guid Guid
        {
            get => Get<Guid>(_DEVICE_GUID_KEY);
            set => Set(_DEVICE_GUID_KEY, value);
        }

        public string Name
        {
            get => Get<string>(_DEVICE_NAME_KEY);
            set => Set(_DEVICE_NAME_KEY, value);
        }

        public string Ip
        {
            get => Get<string>(_SELECTED_COMMS_INTERFACE_KEY);
            set => Set(_SELECTED_COMMS_INTERFACE_KEY, value);
        }

        public int BeaconScanPort
        {
            get => Get<int>(_BEACON_SCAN_PORT);
            set => Set(_BEACON_SCAN_PORT, value);
        }

        public int ScanResponsePort
        {
            get => Get<int>(_SCAN_RESPONSE_PORT);
            set => Set(_SCAN_RESPONSE_PORT, value);
        }
    }
}