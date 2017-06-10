using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDash.Framework.Helpers
{
    public static class XDashHelperMethods
    {
        public static string BytesToString(long byteCount)
        {
            string s;
            const float iKB = 1024;
            const float iMB = 1048576;
            const float iGB = 1073741824;
            if (byteCount < iKB)
                s = string.Format("{0:0.##} B", byteCount);
            else if (byteCount >= iKB && byteCount < iMB)
                s = string.Format("{0:0.##} KB", byteCount / iKB);
            else if (byteCount >= iMB && byteCount < iGB)
                s = string.Format("{0:0.##} MB", byteCount / iMB);
            else s = string.Format("{0:0.##} GB", byteCount / iGB);
            return s;
        }

        /// <summary>
        /// Used to rename a file during the receiving process if the file already exists
        /// and the OverwriteExisting attribue is set to false .
        /// </summary>
        /// <returns>The existing.</returns>
        /// <param name="filepath">Filepath.</param>
        //public static string RenameExisting(string filepath)
        //{
        //    string result = filepath;
        //    string directoryName = System.IO.Path.GetDirectoryName(filepath);
        //    string fileNamePart = System.IO.Path.GetFileNameWithoutExtension(filepath);
        //    string extensionPart = System.IO.Path.GetExtension(filepath);
        //    int currentCount = 1;

        //    while (System.IO.File.Exists(result))
        //    {
        //        result = string.Format("{0} ({1}){2}", fileNamePart, currentCount++, extensionPart);
        //        result = System.IO.Path.Combine(directoryName, result);
        //    }
        //    return result;
        //}
    }
}
