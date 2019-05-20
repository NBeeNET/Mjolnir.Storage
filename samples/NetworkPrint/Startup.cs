using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NBeeNET.Mjolnir.Storage.Local;
using NBeeNET.Mjolnir.Storage.Office;
using NBeeNET.Mjolnir.Storage.Print;
using NBeeNET.Mjolnir.Storage.Job;
using NBeeNET.Mjolnir.Storage.Job.Print;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkPrint
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddHealthChecks()
                .AddCheck<RandomHealthCheck>("random")
                .AddVirtualMemorySizeHealthCheck(10);
            services.AddHealthChecksUI();

            //添加 NBeeNET.Mjolnir.Storage.Image 上传及作业处理服务
            //services.AddStorageImage();
            //添加 NBeeNET.Mjolnir.Storage.Office 上传及作业处理服务
            services.AddStorageOffice()
                .AddJob(new PrintJob().GetType());

            //添加 NBeeNET.Mjolnir.Storage.Local 本地存储服务
            services.AddStorageLocal();
            //添加 NBeeNET.Mjolnir.Storage.Print 打印服务
            services.AddStoragePrint();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            //app.UseHealthChecks("/healthcheck");
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true
            });

            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI();

            app.UseMvc();

          
        }

        public class RandomHealthCheck
        : IHealthCheck
        {
            public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
            {
                if (DateTime.UtcNow.Minute % 2 == 0)
                {
                    return Task.FromResult(HealthCheckResult.Healthy());
                }

                return Task.FromResult(HealthCheckResult.Unhealthy(description: "failed"));
            }
        }
    }
}
