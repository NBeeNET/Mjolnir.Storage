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
        public string SavePath
        {
            get
            {
                return ".//";
            }
        }

        /// <summary>
        /// 返回存储文件夹路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetSavePath(string id)
        {

            string _savePath = Path.Combine(SavePath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), id);

            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            return _savePath;
        }
    }
}
