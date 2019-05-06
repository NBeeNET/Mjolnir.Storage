using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    public class JsonFile
    {

        public JsonFile()
        {
           
        }

        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件Tags
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 子文件
        /// </summary>
        public List<JsonFileValues> Values { get; set; }

        /// <summary>
        /// 保存Json
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task SaveAs(string filePath)
        {
            string jsonStr = JsonConvert.SerializeObject(this);
            await System.IO.File.WriteAllTextAsync(filePath, jsonStr);
        }

        /// <summary>
        /// 读取Json
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<JsonFile> ReadFrom( string filePath)
        {
            string jsonStr = await System.IO.File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<JsonFile>(jsonStr);
        }
        


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
