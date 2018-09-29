using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Kernel;
using Kernel.Actions;
using Kernel.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using WebApi.MapperProfiles;
using Xunit;

namespace Tests
{
    public class TestGettingObjects
    {
        private readonly ExternalSourceOptions _options;

        public TestGettingObjects()
        {
            _options = new ExternalSourceOptions
            {
                GetShowsUri = "http://api.tvmaze.com/shows",
                GetCastsUri = "http://api.tvmaze.com/shows",
                RateLimitPauseSeconds = 10
            };
        }

        private IMapper CreateMapper()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile(new TvShowMapperProfile());
                config.AddProfile(new PersonMapperProfile());
            });
            return Mapper.Instance;
        }

        [Fact]
        public async Task Should_get_shows_page_one()
        {
            var options = Options.Create(_options);
            var logger = Substitute.For<ILogger<GetShowsHandler>>();
            var handler = new GetShowsHandler(options, logger);
            var response = await handler.Handle(new GetShowsRequest(0), CancellationToken.None);
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.True(response.All(x => x.Id <= 250));
        }

        [Fact]
        public async Task Should_get_casts_for_show_1()
        {
            var options = Options.Create(_options);
            var logger = Substitute.For<ILogger<GetCastsHandler>>();
            var handler = new GetCastsHandler(options, logger);
            var response = await handler.Handle(new GetCastsRequest(1), CancellationToken.None);
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.NotNull(response.First().Person);
            Assert.NotNull(response.First().Person.Name);
            Assert.NotNull(response.First().Person.Birthday);
            Assert.NotEqual(new DateTime(1900,1,1), response.First().Person.Birthday);
        }

        [Fact]
        public async Task Should_get_all_shows()
        {
            var options = Options.Create(_options);
            var logger = Substitute.For<ILogger<SynchronizeShowsHandler>>();
            var mapper = CreateMapper();
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<SaveShowsPageRequest>()).Returns(x => x.Arg<SaveShowsPageRequest>().Shows);
            mediator.Send(Arg.Any<GetLastReceivedPageRequest>()).Returns(0);
            mediator.Send(Arg.Is<GetShowsRequest>(req => req.Page == 0)).Returns(new[] { new Show { Id = 1, Name = "first" }, new Show { Id = 2, Name = "Second" } });
            mediator.Send(Arg.Is<GetShowsRequest>(req => req.Page == 1)).Returns((IEnumerable<Show>)null);
            mediator.Send(Arg.Is<GetCastsRequest>(req => req.ShowId == 1)).Returns(new[] { new Cast { Person = new Actor { Id = 1, Name = "Mike Vogel" } }});
            mediator.Send(Arg.Is<GetCastsRequest>(req => req.ShowId == 2)).Returns(new[] { new Cast { Person = new Actor { Id = 2, Name = "Rachelle Lefevre" } }});
            var handler = new SynchronizeShowsHandler(options, mediator, mapper, logger);
            var response = await handler.Handle(new SynchronizeShowsRequest(), CancellationToken.None);
            Assert.NotNull(response);
            Assert.True(response.Count() == 2);
            Assert.NotNull(response.First(item => item.Id == 1).Casts);
            Assert.NotNull(response.First(item => item.Id == 2).Casts);
            Assert.True(response.First(item => item.Id == 1).Casts.Count() == 1);
            Assert.True(response.First(item => item.Id == 2).Casts.Count() == 1);
        }
    }
}
