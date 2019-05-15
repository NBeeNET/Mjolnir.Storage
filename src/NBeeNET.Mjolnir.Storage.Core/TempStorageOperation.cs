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
                string tempDirectory = Path.Combine(Directory.GetCurrentDirectory() , "temp");
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
        public string GetTempPath(string id)
        {
            string tempPath = Path.Combine(BasePath, id);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            return tempPath;
        }

        /// <summary>
        /// 返回Json文件的路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetJsonFilePath(string id)
        {
            string tempPath = GetTempPath(id);
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            string jsonFilePath = Path.Combine(tempPath, id + ".json");

            return jsonFilePath;
        }

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
                string tempDirectory = GetTempPath(id);
                string type = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                string fullFilePath = Path.Combine(tempDirectory, id + "." + type);
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



        /// <summary>
        /// 删除临时文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void Delete(string id)
        {
            try
            {
                //存入临时目录
                string tempDirectory = GetTempPath(id);
                System.IO.Directory.Delete(tempDirectory, true);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }



    }
}
