using System;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace Kernel.Actions
{
    public abstract class AbstractObjectsHandler
    {
        private readonly ILogger _logger;

        protected AbstractObjectsHandler(ILogger logger)
        {
            _logger = logger;
        }

        public virtual async Task<T> Wrap<T>(long timeoutSeconds, Func<Task<T>> action)
        {
            while (true)
            {
                try
                {
                    return await action();
                }
                catch (FlurlHttpTimeoutException fhte)
                {
                    _logger.LogError(fhte.ToString());
                    await Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
                }
                catch (FlurlParsingException fpe)
                {
                    var response = await fpe.GetResponseStringAsync();
                    _logger.LogDebug($"Cannot parse result: {response}");
                    await Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
                }
                catch (FlurlHttpException fhe)
                {
                    int errorCode = fhe.Call.HttpStatus.HasValue ? (int)fhe.Call.HttpStatus.Value : -1;
                    if (errorCode == 404)
                    {
                        return default(T);
                    }
                    if (errorCode == 429)
                    {
                        _logger.LogDebug("Rate limit exceeded. Pausing for 10 seconds");
                        await Task.Delay(TimeSpan.FromSeconds(timeoutSeconds));
                    }
                }
            }
        }
    }
}