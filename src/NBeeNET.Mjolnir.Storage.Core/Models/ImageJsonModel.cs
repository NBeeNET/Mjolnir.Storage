using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// 图片JSON对象
    /// </summary>
    public class ImageJsonModel
    {
        /// <summary>
        /// 唯一值
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 大小
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 分辨率
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// 文件格式
        /// </summary>
        public string Formate { get; set; }

        /// <summary>
        /// 文件所在目录
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }




    }
}
