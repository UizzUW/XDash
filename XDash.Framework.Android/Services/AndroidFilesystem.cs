using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XDash.Framework.Droid.FilePicker;
using XDash.Framework.Services.Contracts.Platform;

namespace XDash.Framework.Droid.Services
{
    public class AndroidFilesystem : IFilesystem
    {
        public async Task<bool> CheckIfFolderExists(string fullPath)
        {
            return await Task.FromResult(Directory.Exists(fullPath));
        }

        public async Task<List<string>> EnumerateFolder(string fullPath)
        {
            if (!Directory.Exists(fullPath))
            {
                return null;
            }
            return await Task.FromResult(new DirectoryInfo(fullPath)
                .EnumerateFiles()
                .Select(f => Path.Combine(fullPath, f.Name))
                .ToList());
        }

        public async Task<ulong> GetFileSize(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return await Task.FromResult((ulong)0);
            }
            return await Task.FromResult((ulong)new FileInfo(fullPath).Length);
        }

        public async Task<Stream> StreamFile(string fullPath)
        {
            return await Task.FromResult(new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite));
        }

        public async Task StreamToFile(string filename, string parentPath, Stream stream)
        {
            if (!await CheckIfFolderExists(parentPath))
            {
                return;
            }
            var fullPath = Path.Combine(parentPath, filename);
            var fileStream = await StreamFile(fullPath);
            await stream.CopyToAsync(fileStream);
            fileStream.Close();
        }

        public async Task<string> ChooseFolder()
        {
            // TODO : replace SFD
            var fileDialog = new SimpleFileDialog(null, SimpleFileDialog.FileSelectionMode.FolderChoose);
            var path = await fileDialog.GetFileOrDirectoryAsync(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
            return path;
        }

        public async Task<List<string>> ChooseAndEnumerateFolder()
        {
            return await EnumerateFolder(await ChooseFolder());
        }

        public async Task<string> ChooseFile()
        {
            return await new FilePickerImplementation().PickFile();
        }

        public async Task<List<string>> ChooseFiles()
        {
            return await new FilePickerImplementation().PickFiles();
        }

        public async Task<Stream> ChooseAndStreamFile()
        {
            return await StreamFile(await ChooseFile());
        }

        public async Task ChooseAndStreamToFile(string filename, Stream stream)
        {
            await StreamToFile(filename, await ChooseFolder(), stream);
        }
    }
}