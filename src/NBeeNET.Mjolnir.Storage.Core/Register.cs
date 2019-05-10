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
        public static List<Core.Interface.IStorageService> _IStorageService = new List<Core.Interface.IStorageService>();

        public static void AddStorage(Core.Interface.IStorageService storageService)
        {
            _IStorageService.Add(storageService);
        }

    }
}
