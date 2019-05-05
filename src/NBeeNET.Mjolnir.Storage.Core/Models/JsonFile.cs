using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    public class JsonFile
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件Tags
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 文件默认存储Url
        /// </summary>
        public string PathUrl { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 子文件
        /// </summary>
        public List<JsonFileValues> Values { get; set; }

        //保存
        //读取


        /// <summary>
        /// 对象转json
        /// </summary>
        /// <returns></returns>
        //public string Save()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}
    }

    public class JsonFileValues
    {
        /// <summary>
        /// 子文件类型Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 子文件参数
        /// </summary>
        public string Param { get; set; }
        /// <summary>
        /// 子文件处理状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 子文件返回值
        /// </summary>
        public string Value { get; set; }
    }
}
