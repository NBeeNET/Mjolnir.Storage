using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBeeNET.Mjolnir.Storage.Job.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Job
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Image 上传作业服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jobType">添加执行任务Job,JobType必须继承IJob</param>
        /// <returns></returns>
        public static IServiceCollection AddJob(this IServiceCollection services, Type jobType)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            IJob job = (IJob)jobType.Assembly.CreateInstance(jobType.FullName);
            Job.RegisterJobs.AddJob(job);

            return services;
        }
    }
}
