using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NBeeNET.Mjolnir.Storage.Core.Jobs
{
    /// <summary>
    /// 复制文件夹
    /// </summary>
    public class CopyDirectoryJob:IJob
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; } = "CopyDirectory";

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var id = context.MergedJobDataMap["id"].ToString();
                TempStorageOperation tempStorage = new TempStorageOperation();
                foreach (var storageService in Register.StorageService)
                {
                    await storageService.CopyDirectory(TempStorageOperation.GetTempPath(id));
                }

                context.Result = new JsonFileDetail()
                {
                    State = "2",
                    Value = "执行成功"
                };
            }
            catch (Exception ex)
            {
                context.Result = new JsonFileDetail()
                {
                    State = "-1",
                    Value = "执行失败：" + ex.Message
                };
            }
            
        }
    }
}
