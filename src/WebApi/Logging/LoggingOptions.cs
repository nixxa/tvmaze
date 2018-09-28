using System.IO;

namespace WebApi.Logging
{
    public class LoggingOptions
    {
        public string LogsPath { get; set; }

        public string GetPath() => Path.GetFullPath(LogsPath ?? "logs");
    }

}