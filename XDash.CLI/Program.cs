using Microsoft.Extensions.DependencyInjection;
using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using XDash.Framework.Components.Discovery;
using XDash.Framework.Components.Discovery.Contracts;
using XDash.Framework.Components.Transfer;
using XDash.Framework.Components.Transfer.Contracts;
using XDash.Framework.Configuration;
using XDash.Framework.Configuration.Contracts;
using XDash.Framework.Core.Services;
using XDash.Framework.Models;
using XDash.Framework.Models.Abstractions;
using XDash.Framework.Services;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Services.Contracts.Platform;
using XDash.Framework.Helpers;

namespace XDash.CLI
{
    public class Program
    {
        private static IServiceProvider _container;

        private struct XDashCLIArgs
        {
            public string Scanner;
            public string Beacon;
            public string Config;
        }

        public static async Task Main(string[] args)
        {
            _container = configureDependencies();
            await configureClient();
            //if (string.IsNullOrEmpty(_container.GetService<IXDashClient>().Ip))
            //{
                await config();
            //}

            var parsedArgs = new XDashCLIArgs();

            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.HandleErrors = false;

                syntax.DefineCommand("scanner", ref parsedArgs.Scanner, "Scan the network for dashers.");
                syntax.DefineCommand("beacon", ref parsedArgs.Beacon, "Wait for dashes from other dashers on the network.");
                syntax.DefineCommand("config", ref parsedArgs.Config, "Configure XDash.");
            });

            if (!string.IsNullOrEmpty(parsedArgs.Scanner))
            {
                await scanner();
            }
            else if (!string.IsNullOrEmpty(parsedArgs.Beacon))
            {
                await beacon();
            }
            else if (!string.IsNullOrEmpty(parsedArgs.Config))
            {
                await config();
            }

            FancyConsole.ReadPadded(1, "Press any key to continue...");
        }

        private static async Task beacon()
        {
            FancyConsole.WritePadded(1, 0, "Waiting for dashers...");
            var beacon = _container.GetService<IBeacon>();
            await beacon.StartListening();
        }

        private static async Task scanner()
        {
            var scanner = _container.GetService<IScanner>();
            var clients = await scanner.Scan();

            if (clients.Any())
            {
                FancyConsole.WritePadded(1, 0, "Clients : ");
                FancyConsole.WritePadded(2, 0, clients.Select(c => c.Ip).ToArray());
            }
            else
            {
                FancyConsole.WritePadded(1, 0, "No clients found.");
            }
        }

        private static async Task config()
        {
            var deviceInfoService = _container.GetService<IDeviceInfoService>();
            var nics = deviceInfoService.GetInterfaces();

            FancyConsole.WritePadded(1, 0, "Interfaces : ");
            FancyConsole.WritePadded(2, 0, nics.Select(nic => nic.GetValidIPv4()).ToArray());

            bool done = false;
            do
            {
                var ip = FancyConsole.ReadPadded(1, "Choice : ");
                var deviceWithIp = nics.FirstOrDefault(nic => nic.GetValidIPv4() == ip);
                if (deviceWithIp != null)
                {
                    done = true;
                    await deviceInfoService.SetSelectedInterface(deviceWithIp);
                }
                else
                {
                    FancyConsole.WritePadded(1, 0, "Invalid option, please try again.");
                }
            } while (!done);
        }

        private static IServiceProvider configureDependencies()
        {
            var collection = new ServiceCollection();

            collection.AddTransient<ILogger, ConsoleLogger>();
            collection.AddTransient<ITimer, Timer>();
            collection.AddSingleton<IJsonSerializer, JsonSerializer>();
            collection.AddSingleton<IBsonSerializer, BsonSerializer>();
            collection.AddSingleton<ICacheService, CacheService>();
            collection.AddSingleton<IConfigurator, Configurator>();
            collection.AddSingleton<IXDashClient, XDashClient>();
            collection.AddSingleton<IDeviceInfoService, DeviceInfoService>();
            collection.AddTransient<IBeacon, Beacon>();
            collection.AddTransient<IScanner, Scanner>();
            collection.AddTransient<IController, Controller>();
            collection.AddTransient<IEndpoint, Endpoint>();
            collection.AddSingleton<IPlatformService, CorePlatformService>();
            collection.AddSingleton<IFilesystem, CoreFilesystem>();

            return collection.BuildServiceProvider();
        }

        private static async Task configureClient()
        {
            var platformService = _container.GetService<IPlatformService>();
            var configurator = _container.GetService<IConfigurator>();
            var deviceInfoService = _container.GetService<IDeviceInfoService>();
            var client = _container.GetService<IXDashClient>();

            await ConfigHelper.ConfigureOptions(client, configurator, platformService, deviceInfoService);
        }
    }
}
