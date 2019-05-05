using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// 任务存储对象
    /// </summary>
    public class FileStrorageModel : FileInfoBase
    {
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// task列表
        /// </summary>
        public List<FileStrorageTaskModel> Tasks { get; set; } = new List<FileStrorageTaskModel>();

        /// <summary>
        /// 对象转json
        /// </summary>
        /// <returns></returns>
        public string ConvertJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// task
    /// </summary>
    public class FileStrorageTaskModel
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 任务参数
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string Status { get; set; }
    }
}
