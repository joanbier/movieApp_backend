using MediatR;
using MovieApp.Application.Dtos;

namespace MovieApp.Application.Queries.Account.GetUserProfile;

public record GetUserProfileQuery() : IRequest<UserDto>;