using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace NBeeNET.Mjolnir.Storage.Job.Implement
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

        public Task Start()
        {
            if (queues.Count > 0)
            {
                IJobExecutionContext jobContext = null;

                while (queues.TryDequeue(out jobContext))
                {
                    this.JobContextList.Add(jobContext);
                    ((IJob)jobContext.JobInstance).Execute(jobContext);
                }

            }
            return Task.CompletedTask;
        }
    }
}
