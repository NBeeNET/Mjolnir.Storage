using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBeeNET.Mjolnir.Storage.Image.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Image
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 Image 上传作业服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="maxLength">设置文件上传最大限制（单位：字节,默认最大为10M）</param>
        /// <returns></returns>
        public static IServiceCollection AddStorageImage(this IServiceCollection services, int maxLength = 0)
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
    }
}
