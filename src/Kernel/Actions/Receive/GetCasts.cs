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
    public class GetCastsHandler : AbstractObjectsHandler, IRequestHandler<GetCastsRequest, IEnumerable<Cast>>
    {
        private readonly ExternalSourceOptions _options;
        private readonly ILogger<GetCastsHandler> _logger;

        public GetCastsHandler(
            IOptions<ExternalSourceOptions> options, 
            ILogger<GetCastsHandler> logger)
            : base(logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Cast>> Handle(GetCastsRequest request, CancellationToken cancellationToken)
        {
            return await Wrap(
                _options.RateLimitPauseSeconds,
                () => {
                    var query = _options.GetCastsUri
                        .AppendPathSegment(request.ShowId)
                        .AppendPathSegment("cast");
                    return query.GetJsonAsync<List<Cast>>(cancellationToken);
                });
        }
    }

    public class GetCastsRequest : IRequest<IEnumerable<Cast>>
    {
        public int ShowId { get; }

        public GetCastsRequest(int showId)
        {
            ShowId = showId;
        }
    }
}