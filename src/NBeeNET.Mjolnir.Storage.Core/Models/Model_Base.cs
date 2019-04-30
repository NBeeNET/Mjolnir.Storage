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
        /// 文件保存Url
        /// </summary>
        public string PathUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
