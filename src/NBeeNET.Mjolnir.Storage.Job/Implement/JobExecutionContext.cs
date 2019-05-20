using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Job.Implement
{
    public class JobExecutionContext : IJobExecutionContext
    {
        private readonly IJobDetail jobDetail;
        private readonly IScheduler scheduler;
        private readonly IJob jobInstance;
        private readonly Hashtable jobDataMap;

        public JobExecutionContext()
        {

        }

        public JobExecutionContext(IJob job, IJobDetail jobDetail)
        {
            this.jobInstance = job;
            this.jobDetail = jobDetail;
            jobDataMap = new Hashtable();
            foreach (DictionaryEntry item in jobDetail.JobDataMap)
            {
                jobDataMap.Add(item.Key, item.Value);
            }
        }

        public JobExecutionContext(IScheduler scheduler, IJob job, IJobDetail jobDetail)
        {
            this.scheduler = scheduler;
            this.jobInstance = job;
            this.jobDetail = jobDetail;
            jobDataMap = new Hashtable();
            foreach (DictionaryEntry item in jobDetail.JobDataMap)
            {
                jobDataMap.Add(item.Key, item.Value);
            }
        }

        public virtual IScheduler Scheduler
        {
            get { return scheduler; }
        }

        public virtual IJobDetail JobDetail => jobDetail;

        public virtual Hashtable MergedJobDataMap => jobDataMap;

        public virtual IJob JobInstance
        {
            get { return jobInstance; }
        }

        public virtual object Result { get; set; }

    }
}
