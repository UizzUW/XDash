﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XDash.Framework.Services.Contracts.Platform
{
    public interface IFilesystem
    {
        Task<bool> CheckIfFolderExists(string fullPath);

        Task<List<string>> EnumerateFolder(string fullPath);
        Task<ulong> GetFileSize(string fullPath);
        Task<Stream> StreamFile(string fullPath);
        Task StreamToFile(string filename, string parentPath, Stream stream);

        Task<string> ChooseFolder();
        Task<List<string>> ChooseAndEnumerateFolder();
        Task<string> ChooseFile();
        Task<List<string>> ChooseFiles();
        Task<Stream> ChooseAndStreamFile();
        Task ChooseAndStreamToFile(string filename, Stream stream);
    }
}
