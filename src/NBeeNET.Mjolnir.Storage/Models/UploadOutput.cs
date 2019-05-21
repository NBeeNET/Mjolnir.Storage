using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Models
{
    /// <summary>
    /// 上传返回结果
    /// </summary>
    public class UploadOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文件名 1.jpg
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Url地址(/NBeeNET/2019/01/01/Guid.jpg)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件相对路径(/NBeeNET/2019/01/01/Guid.jpg)
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件夹相对路径(/NBeeNET/2019/01/01/)
        /// </summary>
        public string Path { get; set; }
    }
}
