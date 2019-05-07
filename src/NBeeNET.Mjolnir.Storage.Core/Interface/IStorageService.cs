using System;
using System.Collections;
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
        /// <param name="destinationDir"></param>
        /// <param name="isOverwriteExisting">是否覆盖</param>
        /// <returns></returns>
        Task<bool> CopyDirectory(string sourceDir, string destinationDir, bool isOverwriteExisting);

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <returns></returns>
        bool MoveDirectory(string sourceDir, string destinationDir);


        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        bool DeleteDirectory(string guid);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetSavePath();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetPath();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetUrl(string filename);

        /// <summary>
        /// 参数
        /// </summary>
        //private Hashtable Options { get; set; }
        
    }
}
