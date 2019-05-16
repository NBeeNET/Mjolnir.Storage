using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// Job输入对象
    /// </summary>
    public class JobInput
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Job类型
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Job处理结果
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否删除临时文件
        /// </summary>
        public bool IsDeleteTemp { get; set; } = true;

        /// <summary>
        /// 上传的文件列表
        /// </summary>
        public List<IFormFile> Files { get; set; }
    }
}
