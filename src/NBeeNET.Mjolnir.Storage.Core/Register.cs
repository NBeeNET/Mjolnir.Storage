using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage
{
    public class Register
    {
        /// <summary>
        /// 是否开启Debug
        /// </summary>
        public static bool IsDebug { get; set; } = true;

        /// <summary>
        /// 存储服务
        /// </summary>
        public static List<Core.Interface.IStorageService> StorageService = new List<Core.Interface.IStorageService>();

        /// <summary>
        /// 存储作业
        /// </summary>
        public static List<Core.Interface.IJob> StorageJobs = new List<Core.Interface.IJob>();

        public static void AddStorage(Core.Interface.IStorageService storageService)
        {
            StorageService.Add(storageService);
        }

        public static void AddStorageJob(Core.Interface.IJob job)
        {
            StorageJobs.Add(job);
        }

    }
}
