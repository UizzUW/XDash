using MVPathway.MVVM.Abstractions;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.Emvy.Services
{
    public class BridgedLogger : ILogger
    {
        private readonly IDiContainer _container;

        public BridgedLogger(IDiContainer container)
        {
            _container = container;
        }

        public void LogInfo(string message)
        {
            var envyLogger = _container.Resolve<MVPathway.Logging.Abstractions.ILogger>();
            envyLogger.LogInfo(message);
        }

        public void LogWarning(string message)
        {
            var envyLogger = _container.Resolve<MVPathway.Logging.Abstractions.ILogger>();
            envyLogger.LogWarning(message);
        }

        public void LogError(string message)
        {
            var envyLogger = _container.Resolve<MVPathway.Logging.Abstractions.ILogger>();
            envyLogger.LogError(message);
        }
    }
}
