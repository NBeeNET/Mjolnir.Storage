using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.AzureBlob
{
    /// <summary>
    /// AzureBlob 存储
    /// </summary>
    public class StorageService : IStorageService
    {
        public string B { get; set; } = "B1";
        
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destinationDir">目标文件夹</param>
        /// <param name="isOverwriteExisting">是否覆盖现有</param>
        /// <returns></returns>
        public async Task<bool> CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting)
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
        
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <returns></returns>
        public bool MoveDirectory(string sourceDir, string destinationDir)
        {
            bool result = false;
            try
            {
                DirectoryInfo destinationDirInfo = new DirectoryInfo(destinationDir);
                if (destinationDirInfo.GetDirectories().Length > 0 || destinationDirInfo.GetFiles().Length > 0)
                {
                    destinationDirInfo.Delete(true);
                    destinationDirInfo.Create();
                }
                Directory.Move(sourceDir, destinationDir);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool DeleteDirectory(string guid)
        {
            bool result = false;
            return result;
        }

        public string GetSavePath()
        {
            throw new NotImplementedException();
        }

        public string GetPath()
        {
            throw new NotImplementedException();
        }

        public string GetUrl(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
