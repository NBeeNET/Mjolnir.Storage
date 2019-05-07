using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// 存储选项参数
    /// </summary>
    public interface IStorageOptions
    {
        /// <summary>
        /// 存储类型
        /// </summary>
        StorageType StorageType { get; set; }

        /// <summary>
        /// 设置保存目录,如：wwwroot
        /// </summary>
        string SavePath { get; set; }

        /// <summary>
        /// 获取保存全路径，如：C://sf/sfs/
        /// </summary>
        /// <returns></returns>
        string GetSavePath();

        string GetUrl(string filename);


        string GetPath();
    }
}
