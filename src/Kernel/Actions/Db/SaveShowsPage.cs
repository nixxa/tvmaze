using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kernel.Dto;
using Kernel.Interfaces;
using LiteDB;
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
                var shows = db.GetCollection<TvShow>();
                var persons = db.GetCollection<Person>();

                var casts = request.Shows.SelectMany(item => item.Casts).Distinct();
                foreach (var person in casts)
                {
                    persons.Upsert(person);
                }
                foreach (var item in request.Shows)
                {
                    shows.Upsert(item);
                }
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