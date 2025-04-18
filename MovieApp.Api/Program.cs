using MovieApp.Application;
using MovieApp.Infrastructure;
using MovieApp.Infrastructure.Auth;
using MovieApp.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

builder.Host.UseInfrastructure();

var app = builder.Build();

await app.Services.SeedRolesAsync();

// Configure the HTTP request pipeline.
app.UsePresentation();

app.UseAuthentication();
app.UseAuthorization();

app.UseApllication();

app.Run();

public partial class Program { }