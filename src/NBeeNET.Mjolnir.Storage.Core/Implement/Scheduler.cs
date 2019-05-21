using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NBeeNET.Mjolnir.Storage.Core.Interface;
using NBeeNET.Mjolnir.Storage.Core.Models;

namespace NBeeNET.Mjolnir.Storage.Core.Implement
{
    public class Scheduler : IScheduler
    {
        private Queue<IJobExecutionContext> queues = null;
        public Scheduler()
        {
            queues = new Queue<IJobExecutionContext>();
        }

        public Scheduler(string schedulerName)
        {
            SchedulerName = schedulerName;
            queues = new Queue<IJobExecutionContext>();
        }

        public string SchedulerName { get; set; }

        public void AddJob(IJobExecutionContext jobContext)
        {
            queues.Enqueue(jobContext);
        }

        public void Clear()
        {
            queues.Clear();
        }

        public IJobDetail GetJobDetail(string jobKey)
        {
            return queues.ToList().Where(t => t.JobDetail.Key == jobKey)?.First().JobDetail;
        }

        public IEnumerable<string> GetJobKeys(string matcher = "")
        {
            return queues.ToList().Where(t => t.JobDetail.Key == matcher).Select(t => t.JobDetail.Key);
        }

        /// <summary>
        /// Job上下文
        /// </summary>
        public List<IJobExecutionContext> JobContextList { get; set; } = new List<IJobExecutionContext>();

        public async Task Start()
        {
            if (queues.Count > 0)
            {

                //循环作业
                while (queues.Count > 0)
                {
                    try
                    {

                        IJobExecutionContext jobContext = queues.Dequeue();
                        this.JobContextList.Add(jobContext);
                        await ((IJob)jobContext.JobInstance).Execute(jobContext);

                        if (jobContext.Result != null)
                        {
                           
                            if (jobContext.JobInstance.Key != "DeleteTemp"|| jobContext.JobInstance.Key != "CopyDirectory")
                            {
                                var id = jobContext.JobDetail.JobDataMap["id"].ToString();
                                var jsonFilePath = System.IO.Path.Combine(TempStorageOperation.BasePath, id) + "\\" + id + ".json";
                                JsonFile JsonFileModel = JsonFile.ReadFrom(jsonFilePath);
                                var result = (JsonFileDetail)jobContext.Result;

                                if (JsonFileModel.Details.Where(t => t.Key == jobContext.JobInstance.Key).Count() > 0)
                                {
                                    var jobModel = JsonFileModel.Details.Where(t => t.Key == jobContext.JobInstance.Key)?.First();
                                    jobModel.Value = result.Value;
                                    jobModel.State = result.State;
                                    //保存Json文件
                                    await JsonFileModel.SaveAs(TempStorageOperation.GetJsonFilePath(id));
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }

        }
    }
}
