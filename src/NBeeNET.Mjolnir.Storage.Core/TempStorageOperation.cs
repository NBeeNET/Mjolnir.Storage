using Microsoft.AspNetCore.Http;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Core
{
    /// <summary>
    /// 临时文件操作
    /// </summary>
    public class TempStorageOperation
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        public string BasePath
        {
            get
            {
                string tempDirectory = Directory.GetCurrentDirectory() + "\\temp";
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }
                return tempDirectory;
            }
        }

        /// <summary>
        /// 返回临时文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetStoragePath(string id)
        {
            string tempPath = Path.Combine(BasePath, id);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }



        ///// <summary>
        ///// 保存路径
        ///// </summary>
        //public string SavePath
        //{
        //    get
        //    {
        //        return  ".//";
        //    }
        //}


        /// <summary>
        /// 返回存储文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public string GetSavePath(string id)
        //{

        //    string _savePath = Path.Combine(SavePath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), id);

        //    if (!Directory.Exists(_savePath))
        //    {
        //        Directory.CreateDirectory(_savePath);
        //    }
        //    return _savePath;
        //}
        /// <summary>
        /// 写入临时文件夹
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> Write(IFormFile file, string id)
        {
            try
            {
                //存入临时目录
                string tempDirectory = GetStoragePath(id);
                string fullFilePath = tempDirectory + id;

                using (var bits = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
                return fullFilePath;
            }
            catch
            {
                return "";
            }
        }

        //删除临时文件夹



        /// <summary>
        /// 写入Json文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public void WriteJsonFile(string id, string jsonStr)
        {
            //存入临时目录
            string tempDirectory = GetStoragePath(id);
            string jsonFilePath = tempDirectory + id + ".json";

            File.WriteAllText(jsonFilePath, jsonStr);

        }

        /// <summary>
        /// 读取Json文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public string ReadJsonFile(string id, string jsonStr)
        {

            //存入临时目录
            string tempDirectory = GetStoragePath(id);
            string jsonFilePath = tempDirectory + id + ".json";

            if (!File.Exists(jsonFilePath))
            {
                return string.Empty;
            }
            return File.ReadAllText(jsonFilePath);
        }

        
    }
}
