using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MovieApp.Application.Middlewares;
using MediatR;
using MovieApp.Application.Commands.Account.ChangeAvatar;
using MovieApp.Application.Commands.Auth.ConfirmEmail;
using MovieApp.Application.Commands.Auth.RegisterUser;
using MovieApp.Application.Commands.Auth.ResetPassword;
using MovieApp.Application.Commands.Comments.AddComment;
using MovieApp.Application.Commands.Movies;
using MovieApp.Application.Commands.Movies.UpdateMovie;
using MovieApp.Application.Common.UserContext;
using MovieApp.Application.Configuration.Validation;

namespace MovieApp.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(executingAssembly));
        services.AddAutoMapper(executingAssembly);
        services.AddHttpContextAccessor();

        services.AddScoped<IUserContext, UserContext>();
        
        services.AddScoped<IValidator<AddMovieCommand>, AddMovieCommandValidation>();
        services.AddScoped<IValidator<UpdateMovieCommand>, UpdateMovieCommandValidation>();
        services.AddScoped<IValidator<AddCommentCommand>, AddCommentCommandValidation>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidation>();
        services.AddScoped<IValidator<ConfirmEmailCommand>, ConfirmEmailCommandValidation>();
        services.AddScoped<IValidator<ResetPasswordCommand>, ResetPasswordCommandValidation>();
        services.AddScoped<IValidator<ChangeAvatarCommand>, ChangeAvatarCommandValidation>();
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));
        services.AddTransient<ExceptionHandlingMiddleware>();
        
        return services;
    }

    public static IApplicationBuilder UseApllication(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        return app;
    }
}