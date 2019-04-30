using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Interface
{
    /// <summary>
    /// 本地存储服务接口
    /// </summary>
    public interface ILocalStorageService
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileJson"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        bool SaveFile(object fileJson, Stream fileStream);

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        object GetFileInfo(string fileGuid);

        /// <summary>
        /// 获取文件的存储路径
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        string GetFilePath(string fileGuid);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        bool DeleteFile(string fileGuid);

    }
}
