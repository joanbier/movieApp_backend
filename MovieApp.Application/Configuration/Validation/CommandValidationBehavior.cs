using FluentValidation;
using FluentValidation.Results;
using MovieApp.Application.Configuration.Commands;
using MediatR;

namespace MovieApp.Application.Configuration.Validation;

public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(!_validators.Any()) 
        { 
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var errorList = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessage) => new ValidationFailure()
                {
                    PropertyName = propertyName,
                    ErrorMessage = string.Join(",", errorMessage.Distinct())
                }
            ).ToList();

        if(errorList.Any())
        {
            throw new ValidationException($"Invalid command, reasons: ", errorList);
        }

        return await next();
    }
}
