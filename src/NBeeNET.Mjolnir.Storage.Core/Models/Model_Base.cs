using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// 基础属性
    /// </summary>
    public class Model_Base
    {
        /// <summary>
        /// guid
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件大小(字节数)
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// 文件保存路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}
