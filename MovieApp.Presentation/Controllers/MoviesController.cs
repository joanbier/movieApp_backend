using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Commands.Movies;
using MovieApp.Application.Commands.Movies.RemoveMovie;
using MovieApp.Application.Commands.Movies.UpdateMovie;
using MovieApp.Application.Dtos;
using MovieApp.Application.Queries.Movies.GetMovieById;
using MovieApp.Application.Queries.Movies.GetMovies;
using MovieApp.Domain.Common;
using MovieApp.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
namespace MovieApp.Presentation.controllers;

[Route("api/movies")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [AllowAnonymous]
    [HttpGet]
    [SwaggerOperation("Get movies", "This Endpoint is public but if user send a token - favorite movie logic will be executed")]
    [ProducesResponseType(typeof(PagedResult<MovieDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetMovies(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] MovieSortBy sortBy = MovieSortBy.Id, 
        [FromQuery] bool descending = false,
        [FromQuery] string searchTerm = "",
        [FromQuery] string genre = ""
        )
    { 
        var movies = await _mediator.Send(new GetMoviesQuery(pageNumber, pageSize, sortBy, descending, searchTerm,  genre));
        return Ok(movies);
    } 
    
    [Authorize]
    [AllowAnonymous]
    [HttpGet("{id}")]
    [SwaggerOperation("Get movie by ID", "This Endpoint is public but if user send a token - favorite movie logic will be executed")]
    [ProducesResponseType(typeof(MovieDetailsDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetMovieById([FromRoute]int id)
    {
        var result = await _mediator.Send(new GetMovieByIdQuery(id));
        return result != null ? Ok(result) : NotFound();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [SwaggerOperation("Add movie")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> AddMovie([FromBody] AddMovieCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetMovieById), new { id = result.Id }, result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut]
    [SwaggerOperation(Summary = "Update movie", Description = "Only Admins can update movies.")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> EditMovie([FromBody] UpdateMovieCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [SwaggerOperation("Remove movie")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeleteMovie([FromRoute]int id)
    {
        await _mediator.Send(new RemoveMovieCommand(id));
        return NoContent();
    }
    
    
}