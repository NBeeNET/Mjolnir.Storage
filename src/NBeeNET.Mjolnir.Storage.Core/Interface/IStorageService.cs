﻿using System;
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

<<<<<<< HEAD
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        T GetInfo<T>(string guid);

        /// <summary>
        /// 获取文件的存储路径
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        string GetPath(string guid);
=======
        bool MoveDirectory(string sourceDir, string destinationDir);
>>>>>>> 85fa6afac25ee48426b5df4c59580d981940f004

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileGuid"></param>
        /// <returns></returns>
        bool Delete(string guid);

    }
}
