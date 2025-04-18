using AutoMapper;
using MediatR;
using MovieApp.Application.Dtos;
using MovieApp.Application.Queries.Movies.GetMovies;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Actors.GetMoviesByActorId;

internal class GetMoviesByActorIdQueryHandler : IRequestHandler<GetMoviesByActorIdQuery, PagedResult<MovieDto>>
{
    
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;

    public GetMoviesByActorIdQueryHandler(IActorRepository actorRepository, IMapper mapper)
    {
        _actorRepository = actorRepository;
        _mapper = mapper;
    }

    public async Task <PagedResult<MovieDto>> Handle(GetMoviesByActorIdQuery request, CancellationToken cancellationToken)
    {
        var pagedMovies = await _actorRepository.GetByIdWithMoviesAsync(request.Id, request.PageNumber, request.PageSize, cancellationToken);
        
        var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(pagedMovies.Items);
        
        return new PagedResult<MovieDto>(moviesDto, pagedMovies.TotalCount, request.PageNumber, request.PageSize);
    }
}