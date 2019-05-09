using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core
{
    public class StorageOperation
    {

        /// <summary>
        /// 获取Url相对全路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetUrl(string filename)
        {
            string url = Path.Combine("NBeeNET_Mjolnir",DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"), filename);
            return url;//.Replace("\\","/");
        }

        /// <summary>
        /// 获取相对目录路径
        /// </summary>
        /// <returns></returns>
        public static string GetPath()
        {

            string path = Path.Combine("NBeeNET_Mjolnir", DateTime.Now.Year.ToString("0000"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
            return path;//.Replace("\\", "/");
        }
    }
}
