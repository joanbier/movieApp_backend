using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Application.Commands.Auth.ConfirmEmail;
using MovieApp.Application.Commands.Auth.ForgotPassword;
using MovieApp.Application.Commands.Auth.LoginUser;
using MovieApp.Application.Commands.Auth.RegisterUser;
using MovieApp.Application.Commands.Auth.ResetPassword;
using MovieApp.Application.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieApp.Presentation.controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    [HttpPost("register")]
    [SwaggerOperation("Register user")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
    {
        await _mediator.Send(command);

        return StatusCode(201);
    }
    
    [HttpPost("confirm-email")]
    [SwaggerOperation("Confirm email")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest(new { message = "Invalid token or user is already confirmed." });

        return Ok(new { message = "E-mail has been confirmed. You can now log in." });
    }
    
    [HttpPost("login")]
    [SwaggerOperation("Login user")]
    [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        
        return Ok(result);
    }
    
    [HttpPost("forgot-password")]
    [SwaggerOperation("Forgot password")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "If this email exists, a reset link has been sent." });
    }
    
    [HttpPost("reset-password")]
    [SwaggerOperation("Reset password")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result)
            return BadRequest(new { message = "Invalid token or email." });

        return Ok(new { message = "Password reset successfully. You can now log in." });
    }
}