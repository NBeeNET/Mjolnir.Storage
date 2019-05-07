using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Core.Common
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public static class CommonHelper
    {

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destinationDir">目标文件夹</param>
        /// <param name="isOverwriteExisting">是否覆盖现有</param>
        /// <returns></returns>
        public static async Task<bool> CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting)
        {
            bool result = false;
            try
            {
                sourceDir = sourceDir.EndsWith(@"\") ? sourceDir : sourceDir + @"\";
                destinationDir = destinationDir.EndsWith(@"\") ? destinationDir : destinationDir + @"\";

                if (Directory.Exists(sourceDir))
                {
                    if (!Directory.Exists(destinationDir))
                        Directory.CreateDirectory(destinationDir);

                    foreach (string file in Directory.GetFiles(sourceDir))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(destinationDir + fileInfo.Name, isOverwriteExisting);
                    }
                    foreach (string dir in Directory.GetDirectories(sourceDir))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                        if (await CopyDirectory(dir, destinationDir + directoryInfo.Name, isOverwriteExisting) == false)
                            result = false;
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
