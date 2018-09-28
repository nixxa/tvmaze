using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kernel.Dto;
using Kernel.Interfaces;
using MediatR;
using Models;

namespace Kernel.Actions
{
    public class SaveShowsPageHandler : IRequestHandler<SaveShowsPageRequest, IEnumerable<TvShow>>
    {
        private readonly IDataProviderFactory _factory;
        private readonly IMapper _mapper;

        public SaveShowsPageHandler(IDataProviderFactory factory, IMapper mapper)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<IEnumerable<TvShow>> Handle(SaveShowsPageRequest request, CancellationToken cancellationToken)
        {
            using (var db = _factory.Create())
            {
                var collection = db.GetCollection<TvShow>();
                collection.InsertBulk(request.Shows);
                return Task.FromResult(request.Shows);
            }
        }
    }

    public class SaveShowsPageRequest : IRequest<IEnumerable<TvShow>>
    {
        public IEnumerable<TvShow> Shows { get; }

        public SaveShowsPageRequest(IEnumerable<TvShow> shows)
        {
            Shows = shows;
        }
    }
}