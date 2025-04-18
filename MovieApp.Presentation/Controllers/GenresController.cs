using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Queries.Genres.GetGenres;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieApp.Presentation.controllers;

[Route("api/genres")]
[ApiController]
public class GenresController : ControllerBase
{
    private readonly IMediator _mediator;

    public GenresController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [SwaggerOperation("Get all genres")]
    [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetGenres()
    {
        var genres = await _mediator.Send(new GetGenresQuery());
        return Ok(genres);
    } 
}