using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NBeeNET.Mjolnir.Storage.Local
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加本地存储服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="basePath">设置本地存储的基础路径（绝对路径,默认为当前项目更目录下wwwroot文件夹）</param>
        /// <returns></returns>
        public static IServiceCollection AddStorageLocal(this IServiceCollection services, string basePath = "")
        {
            if (basePath == "")
            {
                Register.AddStorage(new NBeeNET.Mjolnir.Storage.Local.StorageService() { });

            }
            else
            {
                Register.AddStorage(new NBeeNET.Mjolnir.Storage.Local.StorageService() { BasePath=basePath});
            }
            return services;
        }
    }
}
