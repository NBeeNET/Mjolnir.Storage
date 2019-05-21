using NBeeNET.Mjolnir.Storage.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Core.Implement
{
    public class JobBuilder
    {
        private string key;
        private string name;
        private string description;
        private Type jobType;


        private Hashtable jobDataMap = new Hashtable();

        private IJobDetail JobDetail { get; set; }

        public JobExecutionContext Context { get; set; }

        protected JobBuilder()
        {
        }

        public static JobBuilder Create()
        {
            return new JobBuilder();
        }

        public static JobBuilder Create(Type jobType)
        {
            JobBuilder b = new JobBuilder();
            b.OfType(jobType);
            return b;
        }

        public static JobBuilder Create<T>() where T : IJob
        {
            JobBuilder b = new JobBuilder();
            b.OfType(typeof(T));
            return b;
        }

        public IJobDetail Build()
        {
            JobDetail job = new JobDetail();
            job.Name = name;
            job.JobType = jobType;
            job.Description = description;
            if (key == null)
            {
                key = Guid.NewGuid().ToString();
            }
            job.Key = key;
            job.JobDataMap = jobDataMap;
            return job;
        }

        public JobBuilder OfType(Type type)
        {
            jobType = type;
            return this;
        }

        public JobBuilder WithMutliAttributes(string key, string name, string description, Type type, Hashtable jobDataMap)
        {
            this.key = key;
            this.name = name;
            this.description = description;
            this.jobType = type;
            this.jobDataMap = jobDataMap;
            return this;
        }

        public JobBuilder WithIdentity(string key)
        {
            this.key = key;
            return this;
        }

        public JobBuilder WithName(string name)
        {
            this.name = name;
            return this;
        }

        public JobBuilder AddJobData(string key, object value)
        {
            jobDataMap.Add(key, value);
            return this;
        }

        public JobBuilder SetJobData(string key, object value)
        {
            if (jobDataMap.ContainsKey(key))
            {
                jobDataMap[key] = value;
            }
            return this;
        }


        /// <summary>
        /// 初始化上下文
        /// </summary>
        /// <returns></returns>
        public IJobExecutionContext Initialize()
        {
            this.JobDetail = this.Build();
            IJob job = (IJob)this.jobType.Assembly.CreateInstance(this.jobType.FullName);
            this.Context = new JobExecutionContext(job, this.JobDetail);
            return this.Context;

        }
    }
}
