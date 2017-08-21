using System;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class SettingsRepository : MVPathway.Settings.SettingsRepository, ISettingsRepository
    {
        private const string _DEVICE_NAME_KEY = nameof(_DEVICE_NAME_KEY);
        private const string _DEVICE_GUID_KEY = nameof(_DEVICE_GUID_KEY);
        private const string _SELECTED_COMMS_INTERFACE_KEY = nameof(_SELECTED_COMMS_INTERFACE_KEY);

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
    }
}