using System;
using System.Runtime.InteropServices;
using XDash.Framework.Services.Contracts.Platform;
using OperatingSystem = XDash.Framework.Models.OperatingSystem;

namespace XDash.Framework.Core.Services
{
    public class CorePlatformService : IPlatformService
    {
        public OperatingSystem OS
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return OperatingSystem.Windows;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return OperatingSystem.Linux;
                }
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return OperatingSystem.macOS;
                }
                return OperatingSystem.Unknown;
            }
        }

        public string ConfigurationPath => AppContext.BaseDirectory;

        public void ExitApp()
        {
            Environment.Exit(0);
        }
    }
}
