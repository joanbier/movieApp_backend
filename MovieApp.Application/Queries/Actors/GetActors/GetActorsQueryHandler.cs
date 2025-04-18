using AutoMapper;
using MediatR;
using MovieApp.Application.Dtos;
using MovieApp.Domain.Abstractions;
using MovieApp.Domain.Common;

namespace MovieApp.Application.Queries.Actors.GetActors;

internal class GetActorsQueryHandler : IRequestHandler<GetActorsQuery, PagedResult<ActorDto>>
{
    private readonly IActorRepository _actorRepository;
    private readonly IMapper _mapper;

    public GetActorsQueryHandler(IActorRepository actorRepository, IMapper mapper)
    {
        _actorRepository = actorRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ActorDto>> Handle(GetActorsQuery request, CancellationToken cancellationToken)
    {
        var pagedActors = await _actorRepository.GetAllPagingAsync(request.PageNumber, request.PageSize, cancellationToken);

        var actorsDto = _mapper.Map<IEnumerable<ActorDto>>(pagedActors.Items);

        return new PagedResult<ActorDto>(actorsDto, pagedActors.TotalCount, request.PageNumber, request.PageSize);
    }
}