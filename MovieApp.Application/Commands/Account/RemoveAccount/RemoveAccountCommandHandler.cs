using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Commands;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Auth;

namespace MovieApp.Application.Commands.Account.RemoveAccount;

internal class RemoveAccountCommandHandler : ICommandHandler<RemoveAccountCommand>
{
    private readonly IUserContext _userContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger _logger;

    public RemoveAccountCommandHandler(IUserContext userContext, UserManager<AppUser> userManager, ILogger<RemoveAccountCommandHandler> logger)
    {
        _userContext = userContext;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;

        if (userId == null)
        {
            throw new UnauthorizedAccessException("You are not authorized to add movie comments");
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }
        
        var result = await _userManager.DeleteAsync(user);
      
        _logger.LogDebug($"User with ID {userId} was removed.");

    }
}