using System;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Actions;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kernel.Services
{
    public class SynchronizationService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public SynchronizationService(
            ILogger<SynchronizationService> logger,
            IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Synchronizer is starting");

            if (!stoppingToken.IsCancellationRequested)
            {
                await _mediator.Send(new SynchronizeShowsRequest());
            }

            _logger.LogDebug("Synchronizer is stopping");
        }
    }
}