using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image.ApiControllers.Models
{
    /// <summary>
    /// Image输出结果
    /// </summary>
    public class ImageOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表签
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
        /// Url地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string Path { get; set; }
    }
}
