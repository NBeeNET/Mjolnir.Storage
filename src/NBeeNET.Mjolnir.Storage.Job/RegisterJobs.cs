using NBeeNET.Mjolnir.Storage.Job.Implement;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Job
{
    public sealed class RegisterJobs
    {
        /// <summary>
        /// Job注册
        /// </summary>
        private static List<IJob> jobs= new List<IJob>();

        public RegisterJobs()
        {
            //jobs.Add(new ConvertWebPJob());
            //jobs.Add(new CreateMediumJob());
            //jobs.Add(new ConvertPDFJob());
            //jobs.Add(new CreateSmallJob());
            //jobs.Add(new PrintJob());
        }

        /// <summary>
        /// 添加Job
        /// </summary>
        /// <param name="job"></param>
        public static void AddJob(IJob job)
        {
            jobs.Add(job);
        }

        /// <summary>
        /// 获取job
        /// </summary>
        /// <param name="key"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        public static bool TryGetJob(string key,out IJob job)
        {
            job = jobs.Where(t => t.Key == key)?.First();
            return job == null ? false : true;
        }
    }
}
