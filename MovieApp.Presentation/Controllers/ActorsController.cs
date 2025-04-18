using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Dtos;
using MovieApp.Application.Queries.Actors.GetActors;
using MovieApp.Application.Queries.Actors.GetMoviesByActorId;
using MovieApp.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieApp.Presentation.controllers;

[Route("api/actors")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation("Get actors")]
    [ProducesResponseType(typeof(PagedResult<ActorDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetActors([FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10)
    {
        var actors = await _mediator.Send(new GetActorsQuery(pageNumber, pageSize));
        return Ok(actors);
    } 
    
    [HttpGet("{id}/movies")]
    [SwaggerOperation("Get movies cast by actor with id")]
    [ProducesResponseType(typeof(PagedResult<MovieDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetMoviesByActor([FromRoute]int id, [FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10)
    {
        var movies = await _mediator.Send(new GetMoviesByActorIdQuery(id, pageNumber, pageSize));
        return Ok(movies);
    } 
}