using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        Task<bool> CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting);

        bool MoveDirectory(string sourceDir, string destinationDir);


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        bool Delete(string guid);

    }
}
