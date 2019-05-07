using NBeeNET.Mjolnir.Storage.Core;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.IO;

namespace NBeeNET.Mjolnir.Storage.Local
{
    public class LocalStorageOptions : IStorageOptions
    {
        /// <summary>
        /// 存储类型
        /// </summary>
        public StorageType StorageType { get; set; }

        /// <summary>
        /// 默认
        /// </summary>
        private string _SavePath = "wwwroot";

        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath
        {
            get
            {
                string storageDirectory = Directory.GetCurrentDirectory() + "\\" + _SavePath + "\\";
                if (!Directory.Exists(storageDirectory))
                {
                    Directory.CreateDirectory(storageDirectory);
                }
                return storageDirectory;
            }
            set
            {
                _SavePath = value;
            }
        }

        /// <summary>
        /// 返回存储文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSavePath()
        {

            string _savePath = Path.Combine(SavePath, "NBeeNET_Mjolnir", DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            return _savePath;
        }

        /// <summary>
        /// 获取Url相对全路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetUrl(string filename)
        {
            string url = Path.Combine("NBeeNET_Mjolnir", DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), filename);
            return url;
        }

        /// <summary>
        /// 获取相对目录路径
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {

            string path = Path.Combine("NBeeNET_Mjolnir", DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
            return path;
        }
    }
}
