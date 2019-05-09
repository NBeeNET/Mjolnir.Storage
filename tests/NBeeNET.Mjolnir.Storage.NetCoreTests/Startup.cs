using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NBeeNET.Mjolnir.Storage.NetCoreTests
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

            //services.AddScoped<IStorageService, LocalStorageService>();


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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            //Image.ImageServiceProviders.Providers.Add(
            //    new Image.ServiceProvider() {
            //        Service = new LocalStorageService(),
            //        Options = new Local.LocalStorageOptions() { StorageType= Core.StorageType.Local, SavePath = "wwwroot" } }
            //);
            NBeeNET.Mjolnir.Storage.Image.Operation.Set(new Image.OperationValues() { MaxLength = 1024 * 1024 * 4 });

            NBeeNET.Mjolnir.Storage.Register.AddStorage(new Storage.Local.StorageService() { });
            NBeeNET.Mjolnir.Storage.Register.AddStorage(new Storage.AzureBlob.StorageService() { ConnectionString = "DefaultEndpointsProtocol=https;AccountName=get6;AccountKey=dpC3WSz7aUACwWQ8INEndZZmv0K8T9E1uz9N5WPgDB67FGgWrgUZGjnhzzGkV+xTnQ8Zu+4FfW8Rtl8N9FxljA==;EndpointSuffix=core.chinacloudapi.cn" });
            NBeeNET.Mjolnir.Storage.Register.AddStorage(new Storage.AWSS3.StorageService() { AwsAccessKeyId= "AKIARYB4OSG7FYGB2AOE", AwsSecretAccessKey= "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", BucketName= "nbeenet-mjolnir" });
        }
    }
}
