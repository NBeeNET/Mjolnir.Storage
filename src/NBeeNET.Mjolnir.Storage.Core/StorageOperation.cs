using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core
{
    public class StorageOperation
    {
        /// <summary>
        /// 保存路径
        /// </summary>
        private static string SavePath
        {
            get
            {
                string storageDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\";
                if (!Directory.Exists(storageDirectory))
                {
                    Directory.CreateDirectory(storageDirectory);
                }
                return storageDirectory;
            }
        }

        /// <summary>
        /// 返回存储文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetPath()
        {

            string _savePath = Path.Combine(SavePath,"NBeeNET_Mjolnir",DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            return _savePath;
        }

        public static string GetUrl(string filename)
        {
            string url = Path.Combine("NBeeNET_Mjolnir",DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), filename);
            return url;
        }
    }
}
