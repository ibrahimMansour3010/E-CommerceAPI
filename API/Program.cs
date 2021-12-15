using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace API
{
    class Program
    {
        public static IHostBuilder CreateHostBuilder() 
            => Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webHost => webHost.UseStartup<StartUp>());
        static void Main(string[] args)
        {
            CreateHostBuilder().Build().Run();
        }
    }
}
