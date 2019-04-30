using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// 存储服务接口
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="saveDir"></param>
        /// <returns></returns>
        bool CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting);

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        T GetInfo<T>(string guid);


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        bool Delete(string guid);

    }
}
