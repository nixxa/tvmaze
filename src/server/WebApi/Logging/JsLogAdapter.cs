using JSNLog;

namespace WebApi.Logging
{
    public class JslogAdapter : ILoggingAdapter
    {
        public void Log(FinalLogData finalLogData)
        {
            switch (finalLogData.FinalLevel)
            {
                case Level.WARN:
                    Loggers.Javascript.Warning(finalLogData.FinalMessage);
                    break;
                case Level.ERROR:
                    Loggers.Javascript.Error(finalLogData.FinalMessage);
                    break;
                case Level.FATAL:
                    Loggers.Javascript.Fatal(finalLogData.FinalMessage);
                    break;
                case Level.TRACE:
                    Loggers.Javascript.Debug(finalLogData.FinalMessage);
                    break;
                case Level.DEBUG:
                    Loggers.Javascript.Debug(finalLogData.FinalMessage);
                    break;
                case Level.INFO:
                    Loggers.Javascript.Information(finalLogData.FinalMessage);
                    break;
            }
        }
    }

}