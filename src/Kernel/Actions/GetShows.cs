using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Kernel.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace Kernel.Actions
{
    public class GetShowsHandler : AbstractObjectsHandler, IRequestHandler<GetShowsRequest, IEnumerable<Show>>
    {
        private readonly ExternalSourceOptions _options;
        private readonly ILogger<GetShowsHandler> _logger;

        public GetShowsHandler(
            IOptions<ExternalSourceOptions> options, 
            ILogger<GetShowsHandler> logger)
            : base(logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Show>> Handle(GetShowsRequest request, CancellationToken cancellationToken)
        {
            return await Wrap(
                _options.RateLimitPauseSeconds,
                () => {
                    return _options.GetShowsUri
                        .SetQueryParam("page", request.Page)
                        .GetJsonAsync<List<Show>>(cancellationToken);
                });
        }
    }

    public class GetShowsRequest : IRequest<IEnumerable<Show>>
    {
        public int Page { get; }

        public GetShowsRequest(int page)
        {
            Page = page;
        }
    }
}