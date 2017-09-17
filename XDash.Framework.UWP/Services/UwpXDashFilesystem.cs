using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using XDash.Framework.Services.Contracts;

namespace XDash.Framework.UWP.Services
{
    public class UwpXDashFilesystem : IXDashFilesystem
    {
        public async Task<bool> CheckIfFolderExists(string fullPath)
        {
            var folder = await getFutureAccessListFolder(fullPath);
            return folder == null;
        }

        public async Task<List<string>> EnumerateFolder(string fullPath)
        {
            var folder = await getFutureAccessListFolder(fullPath);
            if (folder == null)
            {
                return null;
            }
            var filesInFolder = await folder.GetFilesAsync();
            return filesInFolder.Select(f => f.Path).ToList();
        }

        public async Task<ulong> GetFileSize(string fullPath)
        {
            var file = await getFutureAccessListFile(fullPath);
            return file == null ? 0 : (await file.GetBasicPropertiesAsync()).Size;
        }

        public async Task<Stream> StreamFile(string fullPath)
        {
            var file = await getFutureAccessListFile(fullPath);
            if (file == null)
            {
                return null;
            }
            return await file.OpenStreamForReadAsync();
        }

        public async Task StreamToFile(string filename, string parentPath, Stream stream)
        {
            var parentFolder = await getFutureAccessListFolder(parentPath);
            if (parentFolder == null)
            {
                return;
            }
            var file = await parentFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            var writeStream = await file.OpenStreamForWriteAsync();
            await stream.CopyToAsync(writeStream);
            writeStream.Dispose();
        }

        public async Task<string> ChooseFolder()
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            var folder = await picker.PickSingleFolderAsync();
            if (folder == null)
            {
                return null;
            }
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(WebUtility.UrlEncode(folder.Path), folder);
            return folder.Path;
        }

        public async Task<List<string>> ChooseAndEnumerateFolder()
        {
            return await EnumerateFolder(await ChooseFolder());
        }

        public async Task<string> ChooseFile()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add("*");

            var file = await picker.PickSingleFileAsync();
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(WebUtility.UrlEncode(file.Path), file);
            return file.Path;
        }

        public async Task<List<string>> ChooseFiles()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add("*");

            var files = await picker.PickMultipleFilesAsync();
            foreach (var file in files)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(WebUtility.UrlEncode(file.Path), file);
            }
            return files.Select(p => p.Path).ToList();
        }

        public async Task<Stream> ChooseAndStreamFile()
        {
            return await StreamFile(await ChooseFile());
        }

        public async Task ChooseAndStreamToFile(string filename, Stream stream)
        {
            await StreamToFile(filename, await ChooseFolder(), stream);
        }

        private async Task<IStorageFile> getFutureAccessListFile(string fullPath)
        {
            var token = WebUtility.UrlEncode(fullPath);
            if (token == null || !StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
            {
                return null;
            }

            return await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
        }

        private async Task<IStorageFolder> getFutureAccessListFolder(string fullPath)
        {
            var token = WebUtility.UrlEncode(fullPath);
            if (token == null || !StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
            {
                return null;
            }

            return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
        }
    }
}
