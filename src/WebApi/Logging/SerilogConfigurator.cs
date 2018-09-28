using System.IO;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Json;

namespace WebApi.Logging
{
    public static class SerilogConfigurator
    {
        public static readonly string DefaultTemplate = "{Timestamp:HH🇲🇲ss} {Level:u4} {SourceContext}: {Message} {Exception}";

        public static ILogger CreateLogger(DirectoryInfo logs)
        {
            return new LoggerConfiguration()
                .ConfigureConsole()
                .ConfigureFiles(logs)
                .CreateLogger();
        }

        public static LoggerConfiguration ConfigureConsole(this LoggerConfiguration self)
        {
            return self.WriteTo.Console();
        }

        public static LoggerConfiguration ConfigureFiles(this LoggerConfiguration self, DirectoryInfo logs)
        {
            return self
                .ConfigureSpecialLogger(nameof(Loggers.Requests), logs)
                .ConfigureSpecialLogger(nameof(Loggers.Javascript), logs)
                .ConfigureCommonLogger(logs);
        }

        private static LoggerConfiguration ConfigureSpecialLogger(this LoggerConfiguration self, string name, DirectoryInfo logs)
        {
            var specialFormatter = new JsonFormatter();
            return self
                .MinimumLevel.Debug()
                .WriteTo
                .Logger(
                    lc => lc
                        .Filter
                        .ByIncludingOnly(Matching.WithProperty(Loggers.SpecialLoggerProperty, name))
                        .WriteTo
                        .RollingFile(specialFormatter, pathFormat: GetPathFormat(name, logs), shared: true));
        }

        private static LoggerConfiguration ConfigureCommonLogger(this LoggerConfiguration self, DirectoryInfo logs)
        {
            var specialFormatter = new JsonFormatter();
            return self
                .WriteTo
                .Logger(
                    lc => lc
                        .Filter
                        .ByExcluding(Matching.WithProperty(Loggers.SpecialLoggerProperty))
                        .WriteTo.RollingFile(specialFormatter, pathFormat: GetPathFormat("tvmaze", logs), shared: true));
        }

        private static string GetPathFormat(string name, DirectoryInfo logs)
        {
            return $"{logs.FullName}\\{name}-{{Date}}.log";
        }
    }

}