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
        public string TempFilePath { get; set; }

        public TempFileOperation()
        {

        }

        /// <summary>
        /// 写入临时文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> WriteTempFile(IFormFile file, string id)
        {
            try
            {
                //存入临时目录
                string rootDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\NBeeNET\\";
                if (!Directory.Exists(rootDirectory))
                {
                    Directory.CreateDirectory(rootDirectory);
                }
                string tempDirectory = rootDirectory + "\\Images\\Temp\\" + id + "\\";
                if (!Directory.Exists(tempDirectory))
                {
                    Directory.CreateDirectory(tempDirectory);
                }
                string fullFilePath = tempDirectory + id + file.ContentType.Split("/")[1];

                using (var bits = new FileStream(fullFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }

                TempFilePath = fullFilePath;
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
            string rootDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\NBeeNET\\";
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }
            string tempDirectory = rootDirectory + "\\Images\\Temp\\" + id + "\\";
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
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
            string rootDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\NBeeNET\\";
            if (!Directory.Exists(rootDirectory))
            {
                Directory.CreateDirectory(rootDirectory);
            }
            string tempDirectory = rootDirectory + "\\Images\\Temp\\" + id + "\\";
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            string jsonFilePath = tempDirectory + id + ".json";

            if (!File.Exists(jsonFilePath))
            {
                return string.Empty;
            }
            return File.ReadAllText(jsonFilePath);
        }
    }
}
