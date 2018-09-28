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
    public class GetShowCastsHandler : AbstractObjectsHandler, IRequestHandler<GetShowCastsRequest, IEnumerable<Cast>>
    {
        private readonly ExternalSourceOptions _options;
        private readonly ILogger<GetShowCastsHandler> _logger;

        public GetShowCastsHandler(
            IOptions<ExternalSourceOptions> options, 
            ILogger<GetShowCastsHandler> logger)
            : base(logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Cast>> Handle(GetShowCastsRequest request, CancellationToken cancellationToken)
        {
            return await Wrap(
                _options.RateLimitPauseSeconds,
                () => {
                    var query = _options.GetShowCastsUri
                        .AppendPathSegment(request.ShowId)
                        .AppendPathSegment("cast");
                    var result = query.GetJsonAsync<List<Cast>>(cancellationToken);
                    return result;
                });
        }
    }

    public class GetShowCastsRequest : IRequest<IEnumerable<Cast>>
    {
        public int ShowId { get; }

        public GetShowCastsRequest(int showId)
        {
            ShowId = showId;
        }
    }
}