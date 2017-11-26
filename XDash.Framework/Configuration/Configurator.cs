using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XDash.Framework.Configuration.Contracts;
using XDash.Framework.Services.Contracts;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Configuration
{
    class Configurator : IConfigurator
    {
        private const string _configFileName = "xdash.config.json";

        private readonly IFilesystem _files;
        private readonly IBsonSerializer _bsonSerializer;

        private string _configurationPath;
        private string _fullConfigurationPath;

        private bool _isSaving;

        public Configurator(IFilesystem files, IBsonSerializer bsonSerializer)
        {
            _files = files;
            _bsonSerializer = bsonSerializer;
        }

        public XDashOptions GetConfiguration()
        {
            while (_isSaving)
            {
                Thread.Sleep(500);
            }

            var config = new ConfigurationBuilder()
               .AddJsonFile(_fullConfigurationPath)
               .Build();

            var options = new XDashOptions();
            config.Bind(options);

            return options;
        }

        public void Init(string configurationPath)
        {
            _configurationPath = configurationPath;
            _fullConfigurationPath = Path.Combine(_configurationPath, _configFileName);
        }

        public async Task SaveConfiguration(XDashOptions options)
        {
            _isSaving = true;

            var serializedOptions = _bsonSerializer.Serialize(options);
            using (var memoryStream = new MemoryStream(serializedOptions))
            {
                await _files.StreamToFile(_configFileName, _configurationPath, memoryStream);
            }
            _isSaving = false;
        }
    }
}
