using System;

namespace XDash.Framework.Services.Contracts
{
    public interface ISettingsRepository : MVPathway.Settings.Abstractions.ISettingsRepository
    {
        Guid Guid { get; set; }
        string Name { get; set; }
        string Ip { get; set; }
        int BeaconScanPort { get; set; }
        int ScanResponsePort { get; set; }
        int TransferPort { get; set; }
        int TransferFeedbackPort { get; set; }
        string DownloadsFolderPath { get; set; }
    }
}
