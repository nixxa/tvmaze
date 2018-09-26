using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebModels;
using AutoMapper;
using Models;
using Core.Interfaces;
using Core.Actions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TvShowsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TvShowsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Getting all shows
        /// </summary>
        /// <returns>List of shows</returns>
        /// <response code="200">Returns all shows</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TvShowModel>))]
        public async Task<ActionResult<IEnumerable<TvShowModel>>> Get([FromQuery] PagingModel paging)
        {
            var request = new GetAllShowsRequest(paging);
            var response = await _mediator.Send(request);
            if (response != null && response.Any())
            {
                return Ok(_mapper.Map<IEnumerable<TvShowModel>>(response));
            }
            return Ok(new TvShowModel[0]);
        }
    }
}
