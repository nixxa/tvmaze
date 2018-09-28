using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Interfaces;
using MediatR;
using Models;

namespace Kernel.Actions
{
    public class GetLastReceivedPageHandler : IRequestHandler<GetLastReceivedPageRequest, int>
    {
        private readonly IDataProviderFactory _factory;

        public GetLastReceivedPageHandler(IDataProviderFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public Task<int> Handle(GetLastReceivedPageRequest request, CancellationToken cancellationToken)
        {
            using (var db = _factory.Create())
            {
                var query = db.GetCollection<TvShow>().FindAll().Select(x => x.Id);
                if (!query.Any())
                {
                    return Task.FromResult(0);
                }
                var result = query.Max() / 250;
                return Task.FromResult(result);
            }
        }
    }

    public class GetLastReceivedPageRequest : IRequest<int>
    {
        
    }
}