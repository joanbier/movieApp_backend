using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Commands.Account.ChangeAvatar;
using MovieApp.Application.Commands.Account.ChangeEmail;
using MovieApp.Application.Commands.Account.ChangePassword;
using MovieApp.Application.Commands.Account.RemoveAccount;
using MovieApp.Application.Dtos;
using MovieApp.Application.Queries.Account.GetUserProfile;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieApp.Presentation.controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet()]
    [SwaggerOperation("Get user profile")]
    [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> GetUserProfile()
    {
        var result = await _mediator.Send(new GetUserProfileQuery());
        return result != null ? Ok(result) : NotFound();
    }
    
    [Authorize]
    [HttpPut("change-email")]
    [SwaggerOperation(Summary = "Change user email", Description = "User can change their email after providing the correct password.")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ChangeEmail([FromBody] ChangeEmailCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest(new { message = "Failed to change email" });

        return Ok(new { message = "Email changed successfully" });
    }
    
    [Authorize]
    [HttpPut("change-password")]
    [SwaggerOperation(Summary = "Change user password", Description = "User can set a new password after providing the current & correct password.")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest(new { message = "Failed to change password" });

        return Ok(new { message = "Password changed successfully" });
    }
    
    [Authorize]
    [HttpPut("change-avatar")]
    [SwaggerOperation(Summary = "Change user avatar", Description = "User can change their avatar.")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ChangeAvatar([FromBody] ChangeAvatarCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest(new { message = "Failed to change avatar" });

        return Ok(new { message = "Avatar changed successfully" });
    }
    
    [Authorize]
    [HttpDelete]
    [SwaggerOperation("Delete account")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<ActionResult> DeleteUser()
    {
        await _mediator.Send(new RemoveAccountCommand());
        return NoContent();
    }
}