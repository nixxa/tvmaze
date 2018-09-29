namespace Kernel
{
    public class ExternalSourceOptions
    {
        public string GetShowsUri { get; set; }
        public string GetCastsUri { get; set; }
        public long RateLimitPauseSeconds { get; set; }
        public int ShowsCacheExpirationHours { get; set; }
    }
}