using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PharmTaxiAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromDays(2);
                        serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(2);
                        serverOptions.Limits.MaxConcurrentConnections = null;
                        serverOptions.Limits.MaxConcurrentUpgradedConnections = null;
                        serverOptions.Limits.MaxRequestBodySize = null;
                        serverOptions.Limits.MinRequestBodyDataRate = null;//new MinDataRate(bytesPerSecond: 100,gracePeriod: TimeSpan.FromSeconds(10));
                        serverOptions.Limits.MinResponseDataRate = null;// new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(5);
                        serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(5);
                    });
                });
    }
}
