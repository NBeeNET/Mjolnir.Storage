using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Job.Implement
{
    public class JobDetail : IJobDetail
    {
        private string key;
        private string name;
        private string description;
        private Type jobType;
        private Hashtable jobDataMap;

        public JobDetail()
        {

        }

        public JobDetail(string name, Type jobType)
        {
            Name = name;
            JobType = jobType;
        }

        public string Key
        {
            get => key;

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    key = Guid.NewGuid().ToString();
                }

                key = value;
            }
        }

        public string Name
        {
            get => name;

            set
            {
                if (value == null || value.Trim().Length == 0)
                {
                    throw new ArgumentException("Job name cannot be empty.");
                }

                name = value;
            }
        }

      

        public string Description
        {
            get => description;
            set => description = value;
        }

        public virtual Type JobType
        {
            get => jobType;
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Job class cannot be null.");
                }

                if (!typeof(IJob).GetTypeInfo().IsAssignableFrom(value.GetTypeInfo()))
                {
                    throw new ArgumentException("Job class must implement the Job interface.");
                }

                jobType = value;
            }
        }

        public virtual Hashtable JobDataMap
        {
            get
            {
                if (jobDataMap == null)
                {
                    jobDataMap = new Hashtable();
                }
                return jobDataMap;
            }

            set => jobDataMap = value;
        }

        public virtual IJobDetail Clone()
        {
            JobDetail copy;
            try
            {
                copy = (JobDetail)MemberwiseClone();
            }
            catch (Exception)
            {
                throw new Exception("Not Cloneable.");
            }

            return copy;
        }

        public virtual JobBuilder GetJobBuilder()
        {
            JobBuilder b = JobBuilder.Create()
                .WithMutliAttributes(key, name, description, jobType, jobDataMap);
            return b;
        }

    }
}
