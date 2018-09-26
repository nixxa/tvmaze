using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using WebApi.Logging;

namespace WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseStaticAssets(this IApplicationBuilder app, IServiceProvider services)
        {
            string logsPath = services.GetService<IOptions<LoggingOptions>>().Value.GetPath();
            var logOptions = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(logsPath),
                RequestPath = new PathString("/logs"),
                ServeUnknownFileTypes = true,
            };
            app.UseStaticFiles(logOptions);
        }

        public static void UseLogBrowsing(this IApplicationBuilder app, IServiceProvider services)
        {
            string logsPath = services.GetService<IOptions<LoggingOptions>>().Value.GetPath();
            var options = new DirectoryBrowserOptions
            {
                RequestPath = "/logs",
                FileProvider = new PhysicalFileProvider(logsPath),
            };
            app.UseDirectoryBrowser(options);
        }
    }
}