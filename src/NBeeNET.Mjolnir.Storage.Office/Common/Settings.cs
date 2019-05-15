using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Office.Common
{
    public class Settings
    {
        /// <summary>
        /// 单次上传文件总大小最大值，默认30M
        /// </summary>
        public int MaxLength { get; set; } = 1024 * 1024 * 30;
    }
}
