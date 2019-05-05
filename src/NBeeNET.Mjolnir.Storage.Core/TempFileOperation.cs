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
    public class TempFileOperation
    {
        /// <summary>
        /// 临时路径
        /// </summary>
        public string TempFilePath
        {
            get
            {
                string tempDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\NBeeNET\\temp";
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }
                return tempDirectory;
            }
        }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath
        {
            get
            {
                return ".//";
            }
        }

        public TempFileOperation()
        {

        }
        /// <summary>
        /// 返回临时文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTempPath(string id)
        {
            string tempPath = Path.Combine(TempFilePath, id);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        /// <summary>
        /// 返回存储文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSavePath(string id)
        {
            string _savePath = Path.Combine(SavePath, id);
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            return _savePath;
        }
        /// <summary>
        /// 写入临时文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> WriteTempFile(IFormFile file, string id,string name)
        {
            try
            {
                //存入临时目录
                string tempDirectory = GetTempPath(id);
                string fullFilePath = tempDirectory + name;

                using (var bits = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
                return fullFilePath;

            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// 写入Json文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public void WriteJsonFile(string id, string jsonStr)
        {
            //存入临时目录
            string tempDirectory = GetTempPath(id);
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
            string tempDirectory = GetTempPath(id);
            string jsonFilePath = tempDirectory + id + ".json";

            if (!File.Exists(jsonFilePath))
            {
                return string.Empty;
            }
            return File.ReadAllText(jsonFilePath);
        }
    }
}
