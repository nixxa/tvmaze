using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NSubstitute;
using WebApi.Controllers;
using Xunit;

namespace Tests
{
    public class TestTvShowsController
    {
        [Fact]
        public async Task Should_return_all_shows()
        {
            var mediator = Substitute.For<IMediator>();
            var mapper = Substitute.For<IMapper>();
            var controller = new TvShowsController(mediator, mapper);

            var result = await controller.Get();
            Assert.NotNull(result);
        }
    }
}
