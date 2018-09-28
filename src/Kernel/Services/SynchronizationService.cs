using System;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Actions;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kernel.Services
{
    public class SynchronizationService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ExternalSourceOptions _options;
        private Timer _timer;

        public SynchronizationService(
            ILogger<SynchronizationService> logger,
            IOptions<ExternalSourceOptions> options,
            IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Synchronizer is starting");

            await _mediator.Send(new SynchronizeShowsRequest());
            CreateTimer(stoppingToken);

            _logger.LogDebug("Synchronizer is stopping");
        }

        private void CreateTimer(CancellationToken stoppingToken)
        {
            _timer = new Timer(
                async state => await ExecuteAsync(stoppingToken), 
                this, 
                TimeSpan.FromHours(_options.ShowsCacheExpirationHours),
                Timeout.InfiniteTimeSpan);
        }
    }
}