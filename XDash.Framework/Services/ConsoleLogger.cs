using System;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Services
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"XDASH INFO : {message}");
        }

        public void LogWarning(string message)
        {
            Console.WriteLine($"XDASH WARNING : {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"XDASH ERROR : {message}");
        }
    }
}
