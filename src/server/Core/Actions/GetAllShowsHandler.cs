using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Interfaces;
using MediatR;
using Models;

namespace Core.Actions
{
    public class GetAllShowsHandler : IRequestHandler<GetAllShowsRequest, IEnumerable<TvShow>>
    {
        private readonly IDataProviderFactory _factory;

        public GetAllShowsHandler(IDataProviderFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        public Task<IEnumerable<TvShow>> Handle(GetAllShowsRequest request, CancellationToken cancellationToken)
        {
            using (var db = _factory.Create())
            {
                var query = db.GetCollection<TvShow>();
                return Task.FromResult(query.FindAll());
            }
        }
    }

    public class GetAllShowsRequest : IRequest<IEnumerable<TvShow>>
    {
        public IPagingParameters Paging { get; }

        public GetAllShowsRequest(IPagingParameters paging)
        {
            Paging = paging;
        }
    }
}