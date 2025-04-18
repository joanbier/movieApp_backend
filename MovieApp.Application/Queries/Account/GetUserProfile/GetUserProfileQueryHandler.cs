using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Entities;
using MovieApp.Domain.Exceptions.Auth;

namespace MovieApp.Application.Queries.Account.GetUserProfile;

internal class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;


    public GetUserProfileQueryHandler(UserManager<AppUser> userManager, IUserContext userContext, IMapper mapper)
    {
        _userManager = userManager;
        _userContext = userContext;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new UnauthorizedAccessException("You are not authorized to this action");
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var userRole = roles.FirstOrDefault() ?? "User";

        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = userRole;

        return userDto;
    }
}