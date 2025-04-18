using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Commands.Favorites.AddFavorite;
using MovieApp.Application.Commands.Favorites.RemoveFavorite;
using MovieApp.Application.Dtos;
using MovieApp.Application.Queries.Favorites.GetAllUserFavMovies;
using MovieApp.Domain.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieApp.Presentation.controllers;

[Route("api/favorites")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FavoritesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet]
    [SwaggerOperation("Get user favorite movies")]
    [ProducesResponseType(typeof(PagedResult<MovieDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetUserFavMovies(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    { 
        var movies = await _mediator.Send(new GetAllUserFavMoviesQuery(pageNumber, pageSize));
        return Ok(movies);
    } 
    
    [Authorize]
    [HttpPost("{movieId}")]
    [SwaggerOperation("Add movie to favorites")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> AddFavorite([FromRoute] int movieId)
    {
        await _mediator.Send(new AddFavoriteCommand(movieId));
        return Ok(new { message = "Movie has been added to favorites" });
    }
    
    [Authorize]
    [HttpDelete("{movieId}")]
    [SwaggerOperation("Remove movie from favorites")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeleteFavorite([FromRoute]int movieId)
    {
        await _mediator.Send(new RemoveFavoriteCommand(movieId));
        return NoContent();
    }
    
}