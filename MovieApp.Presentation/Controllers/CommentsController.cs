using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Commands.Comments.AddComment;
using MovieApp.Application.Commands.Comments.RemoveComment;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieApp.Presentation.controllers;

[Route("api/comments")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpPost("")]
    [SwaggerOperation("Add comment to movie")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> AddComment([FromBody] AddCommentCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Comment added successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [SwaggerOperation("Remove comment from movie")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeleteComment([FromRoute]int id)
    {
        await _mediator.Send(new RemoveCommentCommand(id));
        return NoContent();
    }
}