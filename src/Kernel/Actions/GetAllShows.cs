using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kernel.Interfaces;
using MediatR;
using Models;

namespace Kernel.Actions
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
                var collection = db.GetCollection<TvShow>();
                var query = collection.FindAll();
                if (request.Paging != null)
                {
                    query = query.Skip(request.Paging.Page * request.Paging.PageSize).Take(request.Paging.PageSize);
                }
                return Task.FromResult<IEnumerable<TvShow>>(query.ToList());
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