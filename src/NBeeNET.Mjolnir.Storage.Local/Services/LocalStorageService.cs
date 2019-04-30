using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.IO;

namespace NBeeNET.Mjolnir.Storage.Local.Services
{
    /// <summary>
    /// 本地存储服务
    /// </summary>
    public class LocalStorageService : IStorageService
    {

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destinationDir">目标文件夹</param>
        /// <param name="isOverwriteExisting">是否覆盖现有</param>
        /// <returns></returns>
        public bool CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting)
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
                        if (CopyDirectory(dir, destinationDir + directoryInfo.Name, isOverwriteExisting) == false)
                            result = false;
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destinationDir">目标文件夹</param>
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
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// 查找目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="findStr"></param>
        /// <returns></returns>
        public string FindDirectory(string path,string findStr)
        {
            string directoryPath = "";
            foreach (string dir in Directory.GetDirectories(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                if (directoryInfo.Name != findStr)
                {
                    directoryPath = FindDirectory(dir, findStr);
                }
                else
                {
                    directoryPath = dir;
                }
            }
            return directoryPath;
        }

   

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool Delete(string guid)
        {
            return true;
        }

    }
}
