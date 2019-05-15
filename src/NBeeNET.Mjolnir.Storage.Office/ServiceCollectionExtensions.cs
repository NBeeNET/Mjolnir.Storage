using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NBeeNET.Mjolnir.Storage.Office.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Office
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Office存储服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="maxLength">设置文件上传最大限制（单位：字节,默认最大为30M）</param>
        /// <returns></returns>
        public static IServiceCollection AddStorageOffice(this IServiceCollection services, int maxLength = 0)
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

                    if (configuration.GetSection(Keys.STORAGEOFFICE_SECTION_SETTING_KEY) != null)
                    {
                        configuration.GetSection(Keys.STORAGEOFFICE_SECTION_SETTING_KEY)
                            .Bind(settings, c => c.BindNonPublicProperties = true);
                    }

                });


            return services;
        }
    }
}
