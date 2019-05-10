using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace NBeeNET.Mjolnir.Storage.NetCoreTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseUrls("http://*:5000")
                .UseStartup<Startup>();
    }
}
