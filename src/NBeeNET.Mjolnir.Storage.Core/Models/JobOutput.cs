using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Models
{
    /// <summary>
    /// Job输出对象
    /// </summary>
    public class JobOutput: JsonFileDetail
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// 获取JobModel List
        /// </summary>
        /// <param name="jsonFile"></param>
        /// <returns></returns>
        public static List<JobOutput> GetJobModels(JsonFile jsonFile)
        {
            List<JobOutput> jobModels = new List<JobOutput>();
            if (jsonFile.Details.Count == 0)
            {
                return null;
            }
            foreach (var item in jsonFile.Details)
            {
                jobModels.Add(new JobOutput()
                {
                    Id=jsonFile.Id,
                    FileName=jsonFile.FileName,
                    Url =jsonFile.Url,
                    Key =item.Key,
                    Param=item.Param,
                    State=item.State,
                    CreateTime=item.CreateTime,
                    Value=item.Value
                });
            }

            return jobModels;
        }
    }
}
