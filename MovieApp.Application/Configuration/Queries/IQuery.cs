using MediatR;

namespace MovieApp.Application.Configuration.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
