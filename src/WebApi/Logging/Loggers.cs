using Serilog;

namespace WebApi.Logging
{
    public static class Loggers
    {
        public const string SpecialLoggerProperty = "SpecialLogger";

        public static readonly ILogger Requests = Log.ForContext(SpecialLoggerProperty, nameof(Requests));
        public static readonly ILogger Javascript = Log.ForContext(SpecialLoggerProperty, nameof(Javascript));
    }

}