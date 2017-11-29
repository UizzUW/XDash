using Microsoft.Extensions.Configuration;
using System;
using System.CommandLine;

namespace XDash.CLI
{
    public class XDashOptions
    {
        public XDashDeviceOptions Device { get; set; }

        public XDashDiscoveryOptions Discovery { get; set; }

        public XDashTransferOptions Transfer { get; set; }

        public class XDashDiscoveryOptions
        {
            public int ScanPort { get; set; }

            public int ScanFeedbackPort { get; set; }
        }

        public class XDashTransferOptions
        {
            public int TransferPort { get; set; }

            public int TransferFeedbackPort { get; set; }
        }

        public class XDashDeviceOptions
        {
            public Guid Guid { get; set; }

            public string Name { get; set; }
        }
    }

    public class Program
    {
        private static string address;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("xdash.config.json", optional: true, reloadOnChange: true)
                .Build();

            var options = new XDashOptions();
            config.Bind(options);

            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("n|name", ref address, "The addressee to greet");
            });

            Console.WriteLine("Hello!");
        }
    }
}
