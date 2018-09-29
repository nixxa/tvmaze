using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Flurl;
using Flurl.Http;
using Kernel.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace Kernel.Actions
{
    public class SynchronizeShowsHandler : IRequestHandler<SynchronizeShowsRequest, IEnumerable<TvShow>>
    {
        private readonly ExternalSourceOptions _options;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<SynchronizeShowsHandler> _logger;

        public SynchronizeShowsHandler(
            IOptions<ExternalSourceOptions> options,
            IMediator mediator,
            IMapper mapper,
            ILogger<SynchronizeShowsHandler> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TvShow>> Handle(SynchronizeShowsRequest request, CancellationToken cancellationToken)
        {
            var lastReceivedPage = await _mediator.Send(new GetLastReceivedPageRequest());
            var result = new List<TvShow>();
            _logger.LogInformation($"Synchronizing from page {lastReceivedPage}");
            while (true)
            {
                _logger.LogDebug($"Receiving shows page {lastReceivedPage}");

                var mazeShows = await _mediator.Send(new GetShowsRequest(lastReceivedPage));
                if (mazeShows == null)
                {
                    break;
                }
                var shows = _mapper.Map<IEnumerable<TvShow>>(mazeShows);
                var tasks = shows.Select(SynchronizeCasts);
                await Task.WhenAll(tasks);

                var saved = await _mediator.Send(new SaveShowsPageRequest(shows));
                result.AddRange(saved);
                lastReceivedPage += 1;
            }
            _logger.LogInformation($"All shows synchronized. Last page is {lastReceivedPage}");
            return result;
        }

        private async Task SynchronizeCasts(TvShow show)
        {
            var casts = await _mediator.Send(new GetCastsRequest(show.Id));
            var persons = _mapper.Map<IEnumerable<Person>>(casts);
            show.Casts = persons.ToList();
        }
    }

    public class SynchronizeShowsRequest : IRequest<IEnumerable<TvShow>>
    {
        
    }
}