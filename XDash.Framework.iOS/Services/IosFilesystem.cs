using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.iOS.Services
{
    public class IosFilesystem : IFilesystem
    {
        public Task<bool> CheckIfFolderExists(string fullPath)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<string>> ChooseAndEnumerateFolder()
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> ChooseAndStreamFile()
        {
            throw new System.NotImplementedException();
        }

        public Task ChooseAndStreamToFile(string filename, Stream stream)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ChooseFile()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<string>> ChooseFiles()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> ChooseFolder()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<string>> EnumerateFolder(string fullPath)
        {
            throw new System.NotImplementedException();
        }

        public Task<ulong> GetFileSize(string fullPath)
        {
            throw new System.NotImplementedException();
        }

        public Task<Stream> StreamFile(string fullPath)
        {
            throw new System.NotImplementedException();
        }

        public Task StreamToFile(string filename, string parentPath, Stream stream)
        {
            throw new System.NotImplementedException();
        }
    }
}
