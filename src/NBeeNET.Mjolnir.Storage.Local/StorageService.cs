using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Local
{
    /// <summary>
    /// 本地存储服务
    /// </summary>
    public class StorageService : IStorageService
    {
        /// <summary>
        /// 本地存储的根路径，置绝对路径
        /// </summary>
        public string BasePath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <returns></returns>
        public async Task<bool> CopyDirectory(string sourceDir)
        {
            bool result = false;
            try
            {
                //sourceDir = sourceDir.EndsWith(@"\") ? sourceDir : sourceDir + @"\";
                string destinationDir = Path.Combine(BasePath, StorageOperation.GetPath());

                if (Directory.Exists(sourceDir))
                {
                    if (!Directory.Exists(destinationDir))
                        Directory.CreateDirectory(destinationDir);

                    foreach (string file in Directory.GetFiles(sourceDir))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        fileInfo.CopyTo(Path.Combine(destinationDir, fileInfo.Name), true);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        //Console.WriteLine("Local Url:" + Path.Combine(destinationDir, fileInfo.Name));
                    }
                    foreach (string dir in Directory.GetDirectories(sourceDir))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                        if (await CopyDirectory(dir) == false)
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

        public string GetPath()
        {
            return Core.StorageOperation.GetPath();
        }

        public string GetUrl(string filename)
        {
            return Core.StorageOperation.GetUrl(filename);
        }

        public string GetSavePath()
        {
            string storageDirectory = Path.Combine(BasePath, GetPath());
            if (!Directory.Exists(storageDirectory))
            {
                Directory.CreateDirectory(storageDirectory);
            }
            return storageDirectory;
        }
    }
}
