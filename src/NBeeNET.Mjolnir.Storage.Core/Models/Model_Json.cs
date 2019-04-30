using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// 任务存储对象
    /// </summary>
    public class Model_Json : Model_Base
    {
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// task列表
        /// </summary>
        public List<Model_JsonDetailed> Items { get; set; } = new List<Model_JsonDetailed>();

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
    public class Model_JsonDetailed
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// 任务参数
        /// </summary>
        public List<string> Param { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus { get; set; }
    }
}
