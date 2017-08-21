using System;

namespace XDash.Framework.Services.Contracts
{
    public interface ISettingsRepository : MVPathway.Settings.Abstractions.ISettingsRepository
    {
        Guid Guid { get; set; }
        string Name { get; set; }
        string Ip { get; set; }
    }
}
