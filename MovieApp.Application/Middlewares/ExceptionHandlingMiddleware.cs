using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MovieApp.Domain.Exceptions;

namespace MovieApp.Application.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            title = exception.Message,
            status = statusCode,
            errors = GetErrors(exception)
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    
    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            MovieAppException => (int)(exception as MovieAppException).StatusCode,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception) 
    {
        IReadOnlyDictionary<string, string[]> errors = null;

        if(exception is ValidationException validationException)
        {
            errors = validationException.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage.Split(","));
        }

        return errors;
    }
}