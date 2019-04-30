﻿using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace NBeeNET.Mjolnir.Storage.Local.Services
{
    /// <summary>
    /// 本地存储服务
    /// </summary>
    public class LocalStorageService : IStorageService
    {
        /// <summary>
        /// 默认根目录
        /// </summary>
        public string _rootPath = Directory.GetCurrentDirectory()+"\\wwwroot\\NBeeNet\\Storage\\Images\\";

        public LocalStorageService()
        {

        }

        public LocalStorageService(string rootPath)
        {
            _rootPath = rootPath;
        }

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
        /// 获取文件信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public T GetInfo<T>(string guid)
        {
            try
            {
                //找到guid的文件夹
                string directoryPath = FindDirectory(_rootPath, guid);
                if (directoryPath == "")
                {
                    return null;
                }
                else
                {
                    string content = File.ReadAllText(directoryPath + guid + ".json");
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

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
            foreach (string dir in Directory.GetDirectories(_rootPath))
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
        /// 获取文件的存储路径
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetPath(string guid)
        {
            try
            {
               var infoModel= this.GetInfo<ImageJsonModel>(guid);
                if (infoModel != null)
                {
                    return infoModel.
                }
            }
            catch (Exception)
            {

                throw;
            }
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