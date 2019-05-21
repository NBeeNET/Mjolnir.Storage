using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBeeNET.Mjolnir.Storage.Core.Interface;

namespace NBeeNET.Mjolnir.Storage
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Mjolnir.Storage 服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="maxLength">设置文件上传最大限制（单位：字节,默认最大为10M）</param>
        /// <returns></returns>
        public static IServiceCollection AddStorage(this IServiceCollection services, int maxLength = 0)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            services
                .AddOptions()
                .Configure<Settings>(settings =>
                {
                    if (maxLength != 0)
                    {
                        settings.MaxLength = maxLength;
                    }
                });


            return services;
        }

        /// <summary>
        /// 添加作业服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="jobType">添加执行任务Job,JobType必须继承IJob</param>
        /// <returns></returns>
        public static IServiceCollection AddStorageJob(this IServiceCollection services, IJob job)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            //IJob job = (IJob)jobType.Assembly.CreateInstance(jobType.FullName);
            Register.AddStorageJob(job);

            return services;
        }
    }
}
