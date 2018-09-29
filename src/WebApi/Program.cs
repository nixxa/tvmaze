using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kernel;
using Kernel.Actions;
using Kernel.Data;
using Kernel.Interfaces;
using Kernel.Services;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WebApi.Extensions;
using WebApi.Logging;

namespace WebApi
{
    public class Program
    {
        static IConfiguration Configuration = null;
        public static void Main(string[] args)
        {
            Configuration = CreateConfiguration();
            Log.Logger = CreateLogger(Configuration);
            var builder = CreateWebHostBuilder(args);
            builder
                .UseConfiguration(Configuration)
                .UseSerilog()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureServices(Configure)
                .Build()
                .Run();
        }

        private static IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.AddEnvironmentVariables();
            return builder.Build();
        }

        private static Serilog.ILogger CreateLogger(IConfiguration configuration)
        {
            var options = new LoggingOptions();
            configuration.GetSection("Logging").Bind(options);
            var dir = new DirectoryInfo(options.LogsPath);
            if (!dir.Exists) dir.Create();
            return SerilogConfigurator.CreateLogger(dir);
        }

        private static void Configure(IServiceCollection services)
        {
            // services.AddOptions();
            // services.Configure<DatabaseOptions>(Configuration.GetSection("Database"));
            // services.Configure<ExternalSourceOptions>(Configuration.GetSection("ExternalSource"));

            // services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
            // services.AddMediatR(typeof(GetAllShowsHandler));
            // services.AddAutoMapper();
            // services.AddScoped<SynchronizationService>();
            // services.AddSingleton<IServiceScope>(p => p.CreateScope());
            // services.AddSingleton<IHostedService>(p => {
            //     IServiceScope singletonScope = p.GetService(typeof(IServiceScope)) as IServiceScope;
            //     return singletonScope.ServiceProvider.GetService(typeof(SynchronizationService)) as IHostedService;
            // });
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
