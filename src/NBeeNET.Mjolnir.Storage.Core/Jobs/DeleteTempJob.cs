using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

namespace NBeeNET.Mjolnir.Storage.Core.Jobs
{
    public class DeleteTempJob : IJob
    {
        public string Key { get; set; } = "DeleteTemp";

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var id = context.MergedJobDataMap["id"].ToString();
                //判断Json文件是否存在
                var jsonFilePath = System.IO.Path.Combine(TempStorageOperation.BasePath, id) + "\\" + id + ".json";
                if (File.Exists(jsonFilePath))
                {
                    //获取Json对象
                    JsonFile JsonFileModel = JsonFile.ReadFrom(jsonFilePath);
                    if (JsonFileModel.Details != null && JsonFileModel.Details.Count > 0)
                    {
                        //状态都为2，则表示任务都执行完成
                        if (JsonFileModel.Details.Where(t => t.State == "2").Count() == JsonFileModel.Details.Count)
                        {
                            TempStorageOperation.Delete(id);
                        }
                    }
                    context.Result = new JsonFileDetail()
                    {
                       
                        State = "2",
                        Value = "执行成功"
                    };

                }
            }
            catch (System.Exception ex)
            {
                context.Result = new JsonFileDetail()
                {
                    State = "-1",
                    Value = "执行失败："+ex.Message,
                };
            }
          

        }
    }
}
