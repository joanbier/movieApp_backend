using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using MovieApp.Application.Commands.Auth.RegisterUser;
using MovieApp.Application.Commands.Movies;
using MovieApp.Presentation.Filters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MovieApp.Presentation;


public static class Extensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
            {
                In = ParameterLocation.Header,
                Description = "Please enter token with prefix 'Bearer '",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer",
                
            });

            swagger.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        return services;
    }

    public static IApplicationBuilder UsePresentation(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapControllers();

        return app;
    }
    
    
}